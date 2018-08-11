using Bytes2you.Validation;
using Hemo.Data.Contracts;
using Hemo.Data.Factories;
using Hemo.Models;
using Hemo.Models.Settings;
using Hemo.Utilities;
using Newtonsoft.Json;
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
    public class SettingsController : ApiController
    {
        private IManager usersManager;
        private IUsersFactory usersFactory;
        private IImageExtractor imageExtractor;

        public SettingsController(IUsersManager usersManager, IUsersFactory usersFactory, IImageExtractor imageExtractor)
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

        // PUT api/users/changePassword
        [Authorize]
        [AcceptVerbs("PUT")]
        [HttpPut]
        [Route("api/settings/changePassword")]
        public HttpResponseMessage ChangePassword(ChangePasswordModel model)
        {
            HttpResponseMessage resp = new HttpResponseMessage();

            IEnumerable<User> users = this.usersManager.GetItems() as IEnumerable<User>;
            IEnumerable<Claim> claims = (HttpContext.Current.User as ClaimsPrincipal).Claims;
                
            string userEmail = claims.Where(c => c.Type == ClaimTypes.Email).Select(c => c.Value).FirstOrDefault();

            if (!string.IsNullOrEmpty(userEmail))
            {
                User user = users.Where(u => u.Email == userEmail).FirstOrDefault();

                string hashedOldPassword = PasswordHelper.CreatePasswordHash(model.OldPassword, user.Salt);

                if (user.HashedPassword == hashedOldPassword)
                {
                    string salt = PasswordHelper.CreateSalt(10);
                    string hashedNewPassword = PasswordHelper.CreatePasswordHash(model.NewPassword, salt);

                    user.Salt = salt;
                    user.HashedPassword = hashedNewPassword;

                    this.usersManager.UpdateItem(user);
                    this.usersManager.SaveChanges();

                    resp.Content = new StringContent(JsonConvert.SerializeObject(new ChangeGeneralResponseViewModel() { IsSuccessful = true }));
                    resp.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                }
                else
                {
                    resp.Content = new StringContent(JsonConvert.SerializeObject(new ChangeGeneralResponseViewModel() { IsSuccessful = false, State = "incorrect_password" }));
                    resp.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                }
            }

            return resp;
        }

        // PUT api/users/changeEmail
        [Authorize]
        [AcceptVerbs("PUT")]
        [HttpPut]
        [Route("api/settings/changeEmail")]
        public HttpResponseMessage ChangeEmail(ChangeEmailModel model)
        {
            HttpResponseMessage resp = new HttpResponseMessage();

            IEnumerable<User> users = this.usersManager.GetItems() as IEnumerable<User>;
            IEnumerable<Claim> claims = (HttpContext.Current.User as ClaimsPrincipal).Claims;

            string userEmail = claims.Where(c => c.Type == ClaimTypes.Email).Select(c => c.Value).FirstOrDefault();

            if (!string.IsNullOrEmpty(userEmail))
            {
                User user = users.Where(u => u.Email == userEmail).FirstOrDefault();

                string hashedPassword = PasswordHelper.CreatePasswordHash(model.Password, user.Salt);

                if (user.HashedPassword == hashedPassword)
                {
                    if (users.Where(u => u.Email == model.Email).FirstOrDefault() == null)
                    {
                        user.Email = model.Email;

                        this.usersManager.UpdateItem(user);
                        this.usersManager.SaveChanges();

                        resp.Content = new StringContent(JsonConvert.SerializeObject(new ChangeGeneralResponseViewModel(){ IsSuccessful = true }));
                        resp.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                    }
                    else
                    {
                        resp.Content = new StringContent(JsonConvert.SerializeObject(new ChangeGeneralResponseViewModel() { IsSuccessful = false, State = "existing_mail" }));
                        resp.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                    }
                }
                else
                {
                    resp.Content = new StringContent(JsonConvert.SerializeObject(new ChangeGeneralResponseViewModel() { IsSuccessful = false, State = "incorrect_password" }));
                    resp.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                }
            }

            return resp;
        }

        // PUT api/users/changeLanguage
        [Authorize]
        [AcceptVerbs("PUT")]
        [HttpPut]
        [Route("api/settings/changeLanguage")]
        public HttpResponseMessage ChangeLanguage(ChangeLanguageModel model)
        {
            HttpResponseMessage resp = new HttpResponseMessage();

            IEnumerable<User> users = this.usersManager.GetItems() as IEnumerable<User>;
            IEnumerable<Claim> claims = (HttpContext.Current.User as ClaimsPrincipal).Claims;

            string userEmail = claims.Where(c => c.Type == ClaimTypes.Email).Select(c => c.Value).FirstOrDefault();

            if (!string.IsNullOrEmpty(userEmail))
            {
                User user = users.Where(u => u.Email == userEmail).FirstOrDefault();

                if(user != null)
                {
                    user.PreferredLanguage = (PreferredLanguage)model.SelectedLanguage;

                    this.usersManager.UpdateItem(user);
                    this.usersManager.SaveChanges();

                    resp.Content = new StringContent(JsonConvert.SerializeObject(new ChangeGeneralResponseViewModel() { IsSuccessful = true }));
                    resp.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                }
                else
                {
                    resp.Content = new StringContent(JsonConvert.SerializeObject(new ChangeGeneralResponseViewModel() { IsSuccessful = false }));
                    resp.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                }
            }

            return resp;
        }

        // PUT api/users/changePersonalInformation
        [Authorize]
        [AcceptVerbs("PUT")]
        [HttpPut]
        [Route("api/settings/changePersonalInformation")]
        public HttpResponseMessage ChangePersonalInformation(ChangePersonalInformationModel model)
        {
            HttpResponseMessage resp = new HttpResponseMessage();

            IEnumerable<User> users = this.usersManager.GetItems() as IEnumerable<User>;
            IEnumerable<Claim> claims = (HttpContext.Current.User as ClaimsPrincipal).Claims;

            string userEmail = claims.Where(c => c.Type == ClaimTypes.Email).Select(c => c.Value).FirstOrDefault();

            if (!string.IsNullOrEmpty(userEmail))
            {
                User user = users.Where(u => u.Email == userEmail).FirstOrDefault();

                if (user != null)
                {
                    user.FirstName = model.FirstName;
                    user.LastName = model.LastName;
                    user.PhoneNumber = model.PhoneNumber;
                    user.Age = model.Age;
                    user.BloodType = (BloodType)model.BloodType;
                    user.Image = model.Image;

                    this.usersManager.UpdateItem(user);
                    this.usersManager.SaveChanges();

                    resp.Content = new StringContent(JsonConvert.SerializeObject(new ChangeGeneralResponseViewModel() { IsSuccessful = true }));
                    resp.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                }
                else
                {
                    resp.Content = new StringContent(JsonConvert.SerializeObject(new ChangeGeneralResponseViewModel() { IsSuccessful = false }));
                    resp.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                }
            }

            return resp;
        }

        // PUT api/users/changeLanguage
        [Authorize]
        [AcceptVerbs("PUT")]
        [HttpPut]
        [Route("api/settings/deleteAccount")]
        public HttpResponseMessage DeleteAccount(DeleteAccountModel model)
        {
            HttpResponseMessage resp = new HttpResponseMessage();

            IEnumerable<User> users = this.usersManager.GetItems() as IEnumerable<User>;
            IEnumerable<Claim> claims = (HttpContext.Current.User as ClaimsPrincipal).Claims;

            string userEmail = claims.Where(c => c.Type == ClaimTypes.Email).Select(c => c.Value).FirstOrDefault();

            if (!string.IsNullOrEmpty(userEmail))
            {
                User user = users.Where(u => u.Email == userEmail).FirstOrDefault();

                if (user != null)
                {
                    if (model.IsExternal)
                    {
                        this.usersManager.DeleteItem(user);
                        this.usersManager.SaveChanges();

                        resp.Content = new StringContent(JsonConvert.SerializeObject(new ChangeGeneralResponseViewModel() { IsSuccessful = true }));
                        resp.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                    }
                    else
                    {
                        string hashedPassword = PasswordHelper.CreatePasswordHash(model.Password, user.Salt);

                        if (user.HashedPassword == hashedPassword)
                        {
                            this.usersManager.DeleteItem(user);
                            this.usersManager.SaveChanges();

                            resp.Content = new StringContent(JsonConvert.SerializeObject(new ChangeGeneralResponseViewModel() { IsSuccessful = true }));
                            resp.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                        }
                        else
                        {
                            resp.Content = new StringContent(JsonConvert.SerializeObject(new ChangeGeneralResponseViewModel() { IsSuccessful = false, State = "incorrect_password" }));
                            resp.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                        }
                    } 
                }
                else
                {
                    resp.Content = new StringContent(JsonConvert.SerializeObject(new ChangeGeneralResponseViewModel() { IsSuccessful = false }));
                    resp.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                }
            }

            return resp;
        }
    }
}