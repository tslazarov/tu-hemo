using Bytes2you.Validation;
using Hemo.Data.Contracts;
using Hemo.Models;
using Hemo.Models.Requests;
using Hemo.Models.Trackings;
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
    public class TrackingsController : ApiController
    {
        private IManager usersManager;
        private IManager requestsManager;


        public TrackingsController(IUsersManager usersManager, IDonationsRequestsManager requestsManager)
        {
            Guard.WhenArgument<IUsersManager>(usersManager, "Users manager cannot be null.")
                .IsNull()
                .Throw();
            Guard.WhenArgument<IDonationsRequestsManager>(requestsManager, "Requests manager cannot be null.")
                .IsNull()
                .Throw();

            this.usersManager = usersManager as IManager;
            this.requestsManager = requestsManager as IManager;
        }

        // GET api/trackings
        [Authorize]
        [AcceptVerbs("GET")]
        [HttpGet]
        [Route("api/trackings")]
        public HttpResponseMessage GetTracking()
        {
            TrackingsViewModel trackingViewModel = new TrackingsViewModel();
            trackingViewModel.Locations = new Dictionary<string, int>();

            var startingYear = DateTime.Now.Year - 4;

            trackingViewModel.AnnualDonations = new Dictionary<int, int>();
            trackingViewModel.AnnualDonations.Add(startingYear, 0);
            trackingViewModel.AnnualDonations.Add(startingYear + 1, 0);
            trackingViewModel.AnnualDonations.Add(startingYear + 2, 0);
            trackingViewModel.AnnualDonations.Add(startingYear + 3, 0);
            trackingViewModel.AnnualDonations.Add(startingYear + 4, 0);

            IEnumerable<User> users = this.usersManager.GetItems() as IEnumerable<User>;
            IEnumerable<Claim> claims = (HttpContext.Current.User as ClaimsPrincipal).Claims;

            string userEmail = claims.Where(c => c.Type == ClaimTypes.Email).Select(c => c.Value).FirstOrDefault();

            if (!string.IsNullOrEmpty(userEmail))
            {
                User user = users.Where(u => u.Email == userEmail).FirstOrDefault();

                if (user != null)
                {
                    IEnumerable<DonationsRequest> requests = (this.requestsManager.GetItems() as IEnumerable<DonationsRequest>).OrderByDescending(r => r.Date);

                    foreach (DonationsRequest request in requests)
                    {
                        if (request.Donators.Any(r => r.UserId == user.Id && r.IsApproved))
                        {
                            if(trackingViewModel.LatestRequestDonation == null)
                            {
                                trackingViewModel.LatestRequestDonation = new RequestListViewModel()
                                {
                                    Id = request.Id,
                                    Date = request.Date,
                                    RequestedBloodQuantity = request.RequestedBloodQuantityInMl,
                                    BloodType = request.RequestedBloodType
                                };
                            }

                            if (trackingViewModel.Locations.ContainsKey(request.City))
                            {
                                trackingViewModel.Locations[request.City] += 1;
                            }
                            else
                            {
                                trackingViewModel.Locations.Add(request.City, 1);
                            }

                            if (trackingViewModel.AnnualDonations.ContainsKey(request.Date.Year))
                            {
                                trackingViewModel.AnnualDonations[request.Date.Year] += 1;
                            }
                        }
                    }
                }
            }

            HttpResponseMessage resp = new HttpResponseMessage();

            resp.Content = new StringContent(JsonConvert.SerializeObject(trackingViewModel));
            resp.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            return resp;
        }

        // GET api/trackings/history
        [Authorize]
        [AcceptVerbs("GET")]
        [HttpGet]
        [Route("api/trackings/history")]
        public HttpResponseMessage GetHistory(int skip, int take)
        {
            IList<RequestListViewModel> requestsListViewModel = new List<RequestListViewModel>();

            IEnumerable<User> users = this.usersManager.GetItems() as IEnumerable<User>;
            IEnumerable<Claim> claims = (HttpContext.Current.User as ClaimsPrincipal).Claims;

            string userEmail = claims.Where(c => c.Type == ClaimTypes.Email).Select(c => c.Value).FirstOrDefault();

            IEnumerable<RequestListViewModel> finalQuery = new List<RequestListViewModel>();

            if (!string.IsNullOrEmpty(userEmail))
            {
                User user = users.Where(u => u.Email == userEmail).FirstOrDefault();

                if (user != null)
                {
                    IEnumerable<DonationsRequest> requests = this.requestsManager.GetItems() as IEnumerable<DonationsRequest>;

                    foreach (DonationsRequest request in requests)
                    {
                        if(request.Donators.Any(r => r.UserId == user.Id && r.IsApproved))
                        {
                            requestsListViewModel.Add(new RequestListViewModel()
                            {
                                Id = request.Id,
                                Date = request.Date,
                                RequestedBloodQuantity = request.RequestedBloodQuantityInMl,
                                BloodType = request.RequestedBloodType
                            });
                        }
                    }

                    finalQuery = requestsListViewModel.OrderBy(i => i.Date).Skip(skip).Take(take); 
                }
            }

            HttpResponseMessage resp = new HttpResponseMessage();

            resp.Content = new StringContent(JsonConvert.SerializeObject(finalQuery));
            resp.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            return resp;
        }
    }
}