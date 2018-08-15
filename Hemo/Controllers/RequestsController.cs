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
        private IManager donatorsManager;
        private IDonationsRequestsFactory requestsFactory;
        private IDonatorsFactory donatorsFactory;

        public RequestsController(IDonationsRequestsManager requestsManager, IUsersManager usersManager, IDonatorsManager donatorsManager, IDonationsRequestsFactory requestsFactory, IDonatorsFactory donatorsFactory)
        {
            Guard.WhenArgument<IDonationsRequestsManager>(requestsManager, "Requests manager cannot be null.")
                .IsNull()
                .Throw();
            Guard.WhenArgument<IUsersManager>(usersManager, "Users manager cannot be null.")
                .IsNull()
                .Throw();
            Guard.WhenArgument<IDonatorsManager>(donatorsManager, "Donators manager cannot be null.")
                .IsNull()
                .Throw();
            Guard.WhenArgument<IDonationsRequestsFactory>(requestsFactory, "Requests factory cannot be null.")
                .IsNull()
                .Throw();
            Guard.WhenArgument<IDonatorsFactory>(donatorsFactory, "Donators factory cannot be null.")
                .IsNull()
                .Throw();

            this.requestsManager = requestsManager as IManager;
            this.usersManager = usersManager as IManager;
            this.donatorsManager = donatorsManager as IManager;
            this.requestsFactory = requestsFactory;
            this.donatorsFactory = donatorsFactory;
        }


        // GET api/requests
        [Authorize]
        [AcceptVerbs("GET")]
        [HttpGet]
        [Route("api/requests")]
        public HttpResponseMessage GetRequests(int skip, int take)
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

        // GET api/requests/full
        [Authorize]
        [AcceptVerbs("GET")]
        [HttpGet]
        [Route("api/requests/full")]
        public HttpResponseMessage GetFullRequests(int skip=0, int take=0, decimal latitude=0, decimal longitude=0, bool inRange=false, string city="", string country ="", [FromUri]int[] bloodTypes=null)
        {
            IList<RequestUserListViewModel> requestsListViewModel = new List<RequestUserListViewModel>();

            IEnumerable<User> users = this.usersManager.GetItems() as IEnumerable<User>;
            IEnumerable<Claim> claims = (HttpContext.Current.User as ClaimsPrincipal).Claims;

            string userEmail = claims.Where(c => c.Type == ClaimTypes.Email).Select(c => c.Value).FirstOrDefault();

            if (!string.IsNullOrEmpty(userEmail))
            {
                User user = users.Where(u => u.Email == userEmail).FirstOrDefault();

                if (user != null)
                {
                    IEnumerable<DonationsRequest> requests = this.requestsManager.GetItems() as IEnumerable<DonationsRequest>;

                    IEnumerable<DonationsRequest> query = requests;

                    if (inRange)
                    {
                        query = query.Where(r => RadiusChecker.GetDistance((double)latitude, (double)longitude, (double)r.Latitude, (double)r.Longitude) < 15.0D);
                    }

                    if (!string.IsNullOrEmpty(city) && !string.IsNullOrEmpty(country))
                    {
                        query = query.Where(r => r.City.ToLower() == city.ToLower() && r.Country.ToLower() == country.ToLower());
                    }

                    if (bloodTypes != null && bloodTypes.Length > 0)
                    {
                        query = query.Where(r => bloodTypes.Contains((int)r.RequestedBloodType));
                    }

                    query = query.Skip(skip).Take(take).OrderByDescending(r => r.Date);


                    foreach (var request in query)
                    {
                        User requestUser = this.usersManager.GetItem(request.OwnerId) as User;

                        requestsListViewModel.Add(new RequestUserListViewModel()
                        {
                            Id = request.Id,
                            Date = request.Date,
                            RequestedBloodQuantity = request.RequestedBloodQuantityInMl,
                            BloodType = request.RequestedBloodType,
                            Address = request.Address,
                            Latitude = request.Latitude,
                            Longitude = request.Longitude,
                            Name = string.Format("{0} {1}", requestUser.FirstName, requestUser.LastName),
                            Email = requestUser.Email,
                            PhoneNumber = requestUser.PhoneNumber,
                            Image = requestUser.Image
                        });
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
        [AcceptVerbs("GET")]
        [HttpGet]
        [Route("api/requests/{id}")]
        public HttpResponseMessage GetRequest(Guid id)
        {
            RequestViewModel requestViewModel = new RequestViewModel();

            IEnumerable<User> users = this.usersManager.GetItems() as IEnumerable<User>;
            IEnumerable<Claim> claims = (HttpContext.Current.User as ClaimsPrincipal).Claims;

            bool isSigned;
            string userEmail = claims.Where(c => c.Type == ClaimTypes.Email).Select(c => c.Value).FirstOrDefault();

            if (!string.IsNullOrEmpty(userEmail))
            {
                User user = users.Where(u => u.Email == userEmail).FirstOrDefault();

                if (user != null)
                {
                    DonationsRequest request = this.requestsManager.GetItem(id) as DonationsRequest;

                    if(request.Donators.Any(i => i.UserId == user.Id))
                    {
                        requestViewModel.IsSigned = true;
                    }

                    requestViewModel.Owner = new RequestOwnerViewModel()
                    {
                        Email = user.Email,
                        Name = string.Format("{0} {1}", user.FirstName, user.LastName),
                        PhoneNumber = user.PhoneNumber,
                        Image = user.Image
                    };
                    requestViewModel.Address = request.Address;
                    requestViewModel.Date = request.Date;
                    requestViewModel.Latitude = request.Latitude;
                    requestViewModel.Longitude = request.Longitude;
                    requestViewModel.RequestedBloodQuantityInMl = request.RequestedBloodQuantityInMl;
                    requestViewModel.RequestedBloodType = request.RequestedBloodType;
                }
            }

            HttpResponseMessage resp = new HttpResponseMessage();

            resp.Content = new StringContent(JsonConvert.SerializeObject(requestViewModel));
            resp.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            return resp;
        }

        // POST api/requests/create
        [Authorize]
        [AcceptVerbs("POST")]
        [HttpPost]
        [Route("api/requests/create")]
        public HttpResponseMessage AddUser(RequestsCreateModel model)
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


        // POST api/requests/addUsers
        [Authorize]
        [AcceptVerbs("POST")]
        [HttpPost]
        [Route("api/requests/addDonator")]
        public HttpResponseMessage AddDonatorFromRequest(Guid id)
        {
            bool isAdded = false;

            IEnumerable<User> users = this.usersManager.GetItems() as IEnumerable<User>;
            IEnumerable<Claim> claims = (HttpContext.Current.User as ClaimsPrincipal).Claims;

            string userEmail = claims.Where(c => c.Type == ClaimTypes.Email).Select(c => c.Value).FirstOrDefault();

            if (!string.IsNullOrEmpty(userEmail))
            {
                User user = users.Where(u => u.Email == userEmail).FirstOrDefault();

                if (user != null)
                {
                    DonationsRequest request = this.requestsManager.GetItem(id) as DonationsRequest;

                    Donator donator = this.donatorsFactory.Create(Guid.NewGuid(), user.Email, false);
                    donator.UserId = user.Id;
                    donator.User = user;

                    this.donatorsManager.CreateItem(donator);
                    this.donatorsManager.SaveChanges();


                    request.Donators.Add(donator);
                    this.requestsManager.UpdateItem(request);
                    this.requestsManager.SaveChanges();

                    isAdded = true;
                }
            }

            HttpResponseMessage resp = new HttpResponseMessage();

            resp.Content = new StringContent(JsonConvert.SerializeObject(new ChangeGeneralResponseViewModel() { IsSuccessful = isAdded }));
            resp.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            return resp;
        }

        // POST api/requests/removeUsers
        [Authorize]
        [AcceptVerbs("PUT")]
        [HttpPost]
        [Route("api/requests/removeDonator")]
        public HttpResponseMessage RemoveDonatorFromRequest(Guid id)
        {
            bool isDeleted = false;

            IEnumerable<User> users = this.usersManager.GetItems() as IEnumerable<User>;
            IEnumerable<Claim> claims = (HttpContext.Current.User as ClaimsPrincipal).Claims;

            string userEmail = claims.Where(c => c.Type == ClaimTypes.Email).Select(c => c.Value).FirstOrDefault();

            if (!string.IsNullOrEmpty(userEmail))
            {
                User user = users.Where(u => u.Email == userEmail).FirstOrDefault();

                if (user != null)
                {
                    DonationsRequest request = this.requestsManager.GetItem(id) as DonationsRequest;

                    Donator donator = this.donatorsManager.GetItem(user.Id) as Donator;

                    request.Donators.Remove(donator);
                    this.requestsManager.UpdateItem(request);
                    this.requestsManager.SaveChanges();

                    this.donatorsManager.DeleteItem(donator);
                    this.donatorsManager.SaveChanges();

                    isDeleted = true;
                }
            }

            HttpResponseMessage resp = new HttpResponseMessage();

            resp.Content = new StringContent(JsonConvert.SerializeObject(new ChangeGeneralResponseViewModel() { IsSuccessful = isDeleted }));
            resp.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            return resp;
        }
    }
}