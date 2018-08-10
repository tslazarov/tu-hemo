using Bytes2you.Validation;
using Hemo.Data.Contracts;
using Hemo.Extensions;
using Hemo.Models;
using Hemo.Models.Users;
using Hemo.Utilities;
using Microsoft.Owin.Security.OAuth;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Hemo
{
    public class AuthorizationServerProvider : OAuthAuthorizationServerProvider
    {
        private IManager usersManager;

        public AuthorizationServerProvider(IUsersManager usersManager)
        {
            Guard.WhenArgument<IUsersManager>(usersManager, "Users manager cannot be null.")
                .IsNull()
                .Throw();

            this.usersManager = usersManager as IManager;
        }

        public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            context.Validated();
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            var identity = new ClaimsIdentity(context.Options.AuthenticationType);

            var parameters = context.Request.GetBodyParameters();

            IEnumerable<User> users = this.usersManager.GetItems() as IEnumerable<User>;

            User user = users.FirstOrDefault(u => u.Email == context.UserName);

            if (user != null)
            {
                if (!parameters.ContainsKey("external"))
                {
                    var hashedPassword = PasswordHelper.CreatePasswordHash(context.Password, user.Salt);

                    if (user.HashedPassword == hashedPassword)
                    {
                        identity.AddClaim(new Claim(ClaimTypes.Role, "Admin"));
                        identity.AddClaim(new Claim(ClaimTypes.Email, user.Email));
                        identity.AddClaim(new Claim(ClaimTypes.Name, string.Format("{0} {1}", user.FirstName, user.LastName)));
                        // add additional claims if needed
                        context.Validated(identity);
                    }
                    else
                    {
                        context.SetError("invalid_grant", "Provided username or password is incorrect");
                    }
                }
                else
                {
                    using (var client = new HttpClient())
                    {
                        client.BaseAddress = new Uri(Constants.FacebookGraphAPIBaseUrl);

                        var response = await client.GetAsync(Constants.FacebookGraphAPIMeEndpoint + parameters["access_token"]);
                        var content = await response.Content.ReadAsStringAsync();

                        if(response.StatusCode == HttpStatusCode.OK)
                        {
                            var contentResponse = JsonConvert.DeserializeObject<FacebookModel>(content);

                            if (user.UserExternalId == contentResponse.Id)
                            {
                                identity.AddClaim(new Claim(ClaimTypes.Role, "User"));
                                identity.AddClaim(new Claim(ClaimTypes.Email, user.Email));
                                identity.AddClaim(new Claim(ClaimTypes.Name, string.Format("{0} {1}", user.FirstName, user.LastName)));
                                // add additional claims if needed
                                context.Validated(identity);
                            }
                            else
                            {
                                context.SetError("invalid_grant", "Facebook authorization failed");
                            }
                        }
                    }
                }
            }
            else
            {
                context.SetError("invalid_grant", "Provided username or password is incorrect");
            }
        }
    }
}