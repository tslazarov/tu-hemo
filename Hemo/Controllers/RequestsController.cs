using Bytes2you.Validation;
using Hemo.Data.Contracts;
using Hemo.Data.Factories;
using Hemo.Models;
using Hemo.Models.Requests;
using Hemo.Models.Settings;
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
    public class RequestsController : ApiController
    {
        private IManager usersManager;
        private IManager requestsManager;
        private IDonationsRequestsFactory requestsFactory;
        private IImageExtractor imageExtractor;
        private ISendGridSender sender;


        public RequestsController(IDonationsRequestsManager requestsManager, IUsersManager usersManager, IDonationsRequestsFactory requestsFactory)
        {
            Guard.WhenArgument<IDonationsRequestsManager>(requestsManager, "Requests manager cannot be null.")
                .IsNull()
                .Throw();
            Guard.WhenArgument<IUsersManager>(usersManager, "Users manager cannot be null.")
                .IsNull()
                .Throw();
            Guard.WhenArgument<IDonationsRequestsFactory>(requestsFactory, "Requests factory cannot be null.")
                .IsNull()
                .Throw();

            this.requestsManager = requestsManager as IManager;
            this.usersManager = usersManager as IManager;
            this.requestsFactory = requestsFactory;
        }


        // POST api/requests/create
        [Authorize]
        [AcceptVerbs("GET")]
        [HttpGet]
        [Route("api/requests")]
        public HttpResponseMessage CreateRequests(int skip, int take)
        {
            IList<RequestListViewModel> requestsListViewModel = new List<RequestListViewModel>();

            IEnumerable<User> users = this.usersManager.GetItems() as IEnumerable<User>;
            IEnumerable<Claim> claims = (HttpContext.Current.User as ClaimsPrincipal).Claims;

            string userEmail = claims.Where(c => c.Type == ClaimTypes.Email).Select(c => c.Value).FirstOrDefault();

            if (!string.IsNullOrEmpty(userEmail))
            {
                User user = users.Where(u => u.Email == userEmail).FirstOrDefault();

                if (user != null)
                {
                    IEnumerable<DonationsRequest> requests = this.requestsManager.GetItems() as IEnumerable<DonationsRequest>;

                    IEnumerable<DonationsRequest> query = requests.Where(r => r.OwnerId == user.Id).Skip(skip).Take(take);

                    foreach (var request in query)
                    {
                        requestsListViewModel.Add(new RequestListViewModel() {
                            Id = request.Id,
                            Date = request.Date,
                            RequestedBloodQuantity = request.RequestedBloodQuantityInMl,
                            BloodType = request.RequestedBloodType });
                    }
                }
            }

            HttpResponseMessage resp = new HttpResponseMessage();

            resp.Content = new StringContent(JsonConvert.SerializeObject(requestsListViewModel));
            resp.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            return resp;
        }

        // POST api/requests/create
        [Authorize]
        [AcceptVerbs("POST")]
        [HttpPost]
        [Route("api/requests/create")]
        public HttpResponseMessage CreateRequests(RequestsCreateModel model)
        {
            bool isCreated = false;

            IEnumerable<User> users = this.usersManager.GetItems() as IEnumerable<User>;
            IEnumerable<Claim> claims = (HttpContext.Current.User as ClaimsPrincipal).Claims;

            string userEmail = claims.Where(c => c.Type == ClaimTypes.Email).Select(c => c.Value).FirstOrDefault();

            if (!string.IsNullOrEmpty(userEmail))
            {
                User user = users.Where(u => u.Email == userEmail).FirstOrDefault();

                if (user != null)
                {
                    DonationsRequest request = this.requestsFactory.Create(Guid.NewGuid(), user.Id, model.Address, model.City, model.Country, model.Latitude, model.Longitude, (BloodType)model.RequestedBloodType, model.BloodQuantity);

                    this.requestsManager.CreateItem(request);
                    this.requestsManager.SaveChanges();

                    user.DonationsRequests.Add(request);
                    this.usersManager.UpdateItem(user);
                    this.usersManager.SaveChanges();

                    isCreated = true;
                }
            }

            HttpResponseMessage resp = new HttpResponseMessage();

            resp.Content = new StringContent(JsonConvert.SerializeObject(new ChangeGeneralResponseViewModel() { IsSuccessful = isCreated }));
            resp.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            return resp;
        }
    }
}