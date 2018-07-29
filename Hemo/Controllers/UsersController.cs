using Bytes2you.Validation;
using Hemo.Data.Contracts;
using Hemo.Data.Factories;
using Hemo.Models;
using Hemo.Utilities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Http;

namespace Hemo.Controllers
{
    public class UsersController : ApiController
    {
        private IManager usersManager;
        private IUsersFactory usersFactory;

        public UsersController(IUsersManager usersManager, IUsersFactory usersFactory)
        {
            Guard.WhenArgument<IUsersManager>(usersManager, "Users manager cannot be null.")
                .IsNull()
                .Throw();

            Guard.WhenArgument<IUsersFactory>(usersFactory, "Users factory cannot be null.")
                .IsNull()
                .Throw();

            this.usersManager = usersManager as IManager;
            this.usersFactory = usersFactory;
        }

        // PUT api/users/exist
        [AllowAnonymous]
        [AcceptVerbs("PUT")]
        [HttpPut]
        [Route("api/users/exist")]
        // GET api/<controller>
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

        [AllowAnonymous]
        [AcceptVerbs("POST")]
        [HttpPost]
        [Route("api/users/create")]
        // GET api/<controller>
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
    }
}