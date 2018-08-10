using Hemo.App_Start;
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
        public AuthorizationServerProvider()
        {
        }

        public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            context.Validated();
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            NinjectWrapper wrapper = new NinjectWrapper();

            ClaimsIdentity identity = new ClaimsIdentity(context.Options.AuthenticationType);

            Dictionary<string,string> parameters = context.Request.GetBodyParameters();

            IEnumerable<User> users = ((wrapper.UsersManager as IManager).GetItems() as IEnumerable<User>).ToList();

            User user = users.FirstOrDefault(u => u.Email == context.UserName);

            if (user != null)
            {
                if (!parameters.ContainsKey("external"))
                {
                    string hashedPassword = PasswordHelper.CreatePasswordHash(context.Password, user.Salt);

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
                    using (HttpClient client = new HttpClient())
                    {
                        client.BaseAddress = new Uri(Constants.FacebookGraphAPIBaseUrl);

                        HttpResponseMessage response = await client.GetAsync(Constants.FacebookGraphAPIMeEndpoint + parameters["access_token"]);
                        string content = await response.Content.ReadAsStringAsync();

                        if(response.StatusCode == HttpStatusCode.OK)
                        {
                            FacebookModel contentResponse = JsonConvert.DeserializeObject<FacebookModel>(content);

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