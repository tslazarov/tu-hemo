using Bytes2you.Validation;
using Hemo.Data.Contracts;
using Hemo.Models;
using Hemo.Models.Requests;
using Hemo.Models.Trackings;
using Newtonsoft.Json;
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
        private IManager trackingManager;
        private IManager requestsManager;


        public TrackingsController(IUsersManager usersManager, IUsersDonationTrackingsManager trackingManager, IDonationsRequestsManager requestsManager)
        {
            Guard.WhenArgument<IUsersManager>(usersManager, "Users manager cannot be null.")
                .IsNull()
                .Throw();
            Guard.WhenArgument<IUsersDonationTrackingsManager>(trackingManager, "Trackingmanager cannot be null.")
                .IsNull()
                .Throw();
            Guard.WhenArgument<IDonationsRequestsManager>(requestsManager, "Requests manager cannot be null.")
                .IsNull()
                .Throw();

            this.usersManager = usersManager as IManager;
            this.trackingManager = trackingManager as IManager;
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

            IEnumerable<User> users = this.usersManager.GetItems() as IEnumerable<User>;
            IEnumerable<Claim> claims = (HttpContext.Current.User as ClaimsPrincipal).Claims;

            string userEmail = claims.Where(c => c.Type == ClaimTypes.Email).Select(c => c.Value).FirstOrDefault();

            if (!string.IsNullOrEmpty(userEmail))
            {
                User user = users.Where(u => u.Email == userEmail).FirstOrDefault();

                if (user != null)
                {
                    IEnumerable<UsersDonationTracking> trackings = this.trackingManager.GetItems() as IEnumerable<UsersDonationTracking>;

                    UsersDonationTracking tracking = trackings.FirstOrDefault(t => t.UserId == user.Id);

                    if(tracking != null)
                    {
                        trackingViewModel.LastDonation = tracking.LastDonation;
                        trackingViewModel.CurrentAnnualDonations = tracking.CurrentAnnualDonations;
                    }

                    IEnumerable<DonationsRequest> requests = (this.requestsManager.GetItems() as IEnumerable<DonationsRequest>).OrderByDescending(r => r.Date);

                    foreach (DonationsRequest request in requests)
                    {
                        if (request.Donators.Any(r => r.UserId == user.Id && r.IsApproved))
                        {
                            trackingViewModel.LastRequestDonation = new RequestListViewModel()
                            {
                                Id = request.Id,
                                Date = request.Date,
                                RequestedBloodQuantity = request.RequestedBloodQuantityInMl,
                                BloodType = request.RequestedBloodType
                            };
                            break;
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
        public HttpResponseMessage GetHistory()
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
                }
            }

            HttpResponseMessage resp = new HttpResponseMessage();

            resp.Content = new StringContent(JsonConvert.SerializeObject(requestsListViewModel));
            resp.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            return resp;
        }
    }
}