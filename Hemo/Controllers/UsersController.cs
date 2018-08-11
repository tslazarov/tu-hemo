using Bytes2you.Validation;
using Hemo.Data.Contracts;
using Hemo.Data.Factories;
using Hemo.Models;
using Hemo.Models.Users;
using Hemo.SendGrid;
using Hemo.Utilities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Web;
using System.Web.Http;

namespace Hemo.Controllers
{
    public class UsersController : ApiController
    {
        private IManager usersManager;
        private IUsersFactory usersFactory;
        private IImageExtractor imageExtractor;
        private ISendGridSender sender;


        public UsersController(IUsersManager usersManager, IUsersFactory usersFactory, IImageExtractor imageExtractor, ISendGridSender sender)
        {
            Guard.WhenArgument<IUsersManager>(usersManager, "Users manager cannot be null.")
                .IsNull()
                .Throw();

            Guard.WhenArgument<IUsersFactory>(usersFactory, "Users factory cannot be null.")
                .IsNull()
                .Throw();

            Guard.WhenArgument<IImageExtractor>(imageExtractor, "Image extractor cannot be null.")
                .IsNull()
                .Throw();
            Guard.WhenArgument<ISendGridSender>(sender, "SendGrid Sender cannot be null.")
                .IsNull()
                .Throw();

            this.usersManager = usersManager as IManager;
            this.usersFactory = usersFactory;
            this.imageExtractor = imageExtractor;
            this.sender = sender;
        }

        // PUT api/users/exist
        [AllowAnonymous]
        [AcceptVerbs("PUT")]
        [HttpPut]
        [Route("api/users/exist")]
        public HttpResponseMessage CheckExistingEmail(UsersExistingEmailModel model)
        {
            HttpResponseMessage resp = new HttpResponseMessage();

            IEnumerable<User> users = this.usersManager.GetItems() as IEnumerable<User>;

            if(users != null)
            {
                resp.Content = new StringContent(JsonConvert.SerializeObject(users.Any(u => u.Email == model.Email)));
                resp.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            }

            return resp;
        }

        // POST api/users/create
        [AllowAnonymous]
        [AcceptVerbs("POST")]
        [HttpPost]
        [Route("api/users/create")]
        public HttpResponseMessage CreateUser(UsersCreateModel model)
        {
            bool existingUser = false;
            bool isCreated = false;
            string salt;
            string hashedPassword;

            IEnumerable<User> users = ((IManager)this.usersManager).GetItems() as IEnumerable<User>;

            if (users.Any(u => u.Email == model.Email))
            {
                existingUser = true;
            }

            if (!existingUser)
            {
                User user = this.usersFactory.Create(Guid.NewGuid(), model.Email, model.FirstName, model.LastName, model.IsExternal);

                if (model.IsExternal)
                {
                    user.Image = imageExtractor.GetImageAsBase64Url(model.AccessToken).Result;
                    salt = string.Empty;
                    hashedPassword = string.Empty;
                }
                else
                {
                    salt = PasswordHelper.CreateSalt(10);
                    hashedPassword = PasswordHelper.CreatePasswordHash(model.Password, salt);
                }

                user.Salt = salt;
                user.HashedPassword = hashedPassword;
                user.UserExternalId = model.UserExternalId;
                user.Age = model.Age;
                user.PhoneNumber = model.PhoneNumber;
                user.BloodType = (BloodType)model.BloodType;

                if(model.PreferredLanguage == "en")
                {
                    user.PreferredLanguage = PreferredLanguage.English;
                }
                else if(model.PreferredLanguage == "bg")
                {
                    user.PreferredLanguage = PreferredLanguage.Bulgarian;
                }

                this.usersManager.CreateItem(user);
                this.usersManager.SaveChanges();
                isCreated = true;
            }

            HttpResponseMessage resp = new HttpResponseMessage();

            if (users != null)
            {
                resp.Content = new StringContent(JsonConvert.SerializeObject(isCreated));
                resp.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            }

            return resp;
        }

        // PUT api/users/resetPassword
        [AllowAnonymous]
        [AcceptVerbs("PUT")]
        [HttpPut]
        [Route("api/users/resetPassword")]
        public HttpResponseMessage ResetPassword(UsersResetPasswordModel model)
        {
            HttpResponseMessage resp = new HttpResponseMessage();

            IEnumerable<User> users = this.usersManager.GetItems() as IEnumerable<User>;

            User user = users.FirstOrDefault(u => u.Email == model.Email && !u.IsExternal);

            if (user != null)
            {
                string randomNumber = Guid.NewGuid().ToString().Replace("-", "").Substring(12, 8);

                string plainText = sender.GetResetPasswordPlainText(randomNumber);
                string htmlText = sender.GetResetPasswordHtml(randomNumber);

                sender.SendMessage("support@hemo.com", "Hemo Support", model.Email, "[Hemo] Password Reset", plainText, htmlText);

                resp.Content = new StringContent("test");
                resp.StatusCode = HttpStatusCode.OK;
                resp.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            }

            return resp;
        }

        // GET api/users/basicProfile
        [Authorize]
        [AcceptVerbs("GET")]
        [HttpGet]
        [Route("api/users/basicProfile")]
        public HttpResponseMessage GetBasicProfile()
        {
            ClaimsPrincipal user = HttpContext.Current.User as ClaimsPrincipal;

            UsersBasicProfileViewModel viewModel = new UsersBasicProfileViewModel();

            if(user !=null)
            {
                foreach (Claim claim in user.Claims)
                {
                    if(claim.Type == ClaimTypes.Email)
                    {
                        viewModel.Email = claim.Value;

                    }
                    else if(claim.Type == ClaimTypes.Name)
                    {
                        viewModel.Name = claim.Value;
                    }
                }

                IEnumerable<User> users = ((IManager)this.usersManager).GetItems() as IEnumerable<User>;

                User hemoUser = users.Where(u => u.Email == viewModel.Email).FirstOrDefault();

                if(hemoUser != null)
                {
                    viewModel.ProfileImage = hemoUser.Image;
                    viewModel.IsExternal = hemoUser.IsExternal;
                }
                
            }

            HttpResponseMessage resp = new HttpResponseMessage();

            resp.Content = new StringContent(JsonConvert.SerializeObject(viewModel));
            resp.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            return resp;
        }

        // GET api/users/fullProfile
        [Authorize]
        [AcceptVerbs("GET")]
        [HttpGet]
        [Route("api/users/fullProfile")]
        public HttpResponseMessage GetFullProfile()
        {
            ClaimsPrincipal user = (HttpContext.Current.User as ClaimsPrincipal);
            string userEmail = user.Claims.Where(c => c.Type == ClaimTypes.Email).Select(c => c.Value).FirstOrDefault();

            UsersFullProfileViewModel viewModel = new UsersFullProfileViewModel();

            if (user != null)
            {
                IEnumerable<User> users = ((IManager)this.usersManager).GetItems() as IEnumerable<User>;

                User hemoUser = users.Where(u => u.Email == userEmail).FirstOrDefault();

                if (hemoUser != null)
                {
                    viewModel.Email = hemoUser.Email;
                    viewModel.FirstName = hemoUser.FirstName;
                    viewModel.LastName = hemoUser.LastName;
                    viewModel.PhoneNumber = hemoUser.PhoneNumber;
                    viewModel.Age = hemoUser.Age;
                    viewModel.BloodType = (int)hemoUser.BloodType;
                    viewModel.ProfileImage = hemoUser.Image;
                }

            }

            HttpResponseMessage resp = new HttpResponseMessage();

            resp.Content = new StringContent(JsonConvert.SerializeObject(viewModel));
            resp.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            return resp;
        }

        // GET api/users/preferredLanguage
        [Authorize]
        [AcceptVerbs("GET")]
        [HttpGet]
        [Route("api/users/preferredLanguage")]
        public HttpResponseMessage GetPreferredLanguage()
        {
            ClaimsPrincipal user = HttpContext.Current.User as ClaimsPrincipal;

            PreferredLanguageViewModel viewModel = new PreferredLanguageViewModel();

            IEnumerable<User> users = this.usersManager.GetItems() as IEnumerable<User>;
            IEnumerable<Claim> claims = (HttpContext.Current.User as ClaimsPrincipal).Claims;

            string userEmail = claims.Where(c => c.Type == ClaimTypes.Email).Select(c => c.Value).FirstOrDefault();

            if (!string.IsNullOrEmpty(userEmail))
            {
                User hemoUser = users.Where(u => u.Email == userEmail).FirstOrDefault();

                if (hemoUser != null)
                {
                    viewModel.PreferredLanguage = (int)hemoUser.PreferredLanguage;
                }
            }

            HttpResponseMessage resp = new HttpResponseMessage();

            resp.Content = new StringContent(JsonConvert.SerializeObject(viewModel));
            resp.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            return resp;
        }
    }
}