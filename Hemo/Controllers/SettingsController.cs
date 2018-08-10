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

        // PUT api/users/exist
        [Authorize]
        [AcceptVerbs("PUT")]
        [HttpPut]
        [Route("api/settings/changePassword")]
        public HttpResponseMessage CheckExistingEmail(ChangePasswordModel model)
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

                    this.usersManager.SaveChanges();

                    resp.Content = new StringContent(JsonConvert.SerializeObject(true));
                    resp.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                }
                else
                {
                    resp.Content = new StringContent(JsonConvert.SerializeObject(false));
                    resp.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                }
            }

            return resp;
        }
    }
}