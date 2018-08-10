using Bytes2you.Validation;
using Hemo.Data.Contracts;
using Hemo.Data.Factories;
using Hemo.Models;
using Hemo.Models.Users;
using Hemo.Utilities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public UsersController(IUsersManager usersManager, IUsersFactory usersFactory, IImageExtractor imageExtractor)
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

            this.usersManager = usersManager as IManager;
            this.usersFactory = usersFactory;
            this.imageExtractor = imageExtractor;
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

        // GET api/users/basicProfile
        [Authorize]
        [AcceptVerbs("GET")]
        [HttpGet]
        [Route("api/users/basicProfile")]
        public HttpResponseMessage GetBasicProfile()
        {
            ClaimsPrincipal user = HttpContext.Current.User as ClaimsPrincipal;

            UsersBasicProfileViewModel profile = new UsersBasicProfileViewModel();

            if(user !=null)
            {
                foreach (Claim claim in user.Claims)
                {
                    if(claim.Type == ClaimTypes.Email)
                    {
                        profile.Email = claim.Value;

                    }
                    else if(claim.Type == ClaimTypes.Name)
                    {
                        profile.Name = claim.Value;
                    }
                }

                IEnumerable<User> users = ((IManager)this.usersManager).GetItems() as IEnumerable<User>;

                var hemoUser = users.Where(u => u.Email == profile.Email).FirstOrDefault();

                if(hemoUser != null)
                {
                    profile.ProfileImage = hemoUser.Image;
                    profile.IsExternal = hemoUser.IsExternal;
                }
                
            }

            HttpResponseMessage resp = new HttpResponseMessage();

            resp.Content = new StringContent(JsonConvert.SerializeObject(profile));
            resp.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            return resp;
        }
    }
}