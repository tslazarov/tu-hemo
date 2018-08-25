using Bytes2you.Validation;
using Hemo.Data.Contracts;
using Hemo.Data.Factories;
using Hemo.Models;
using Hemo.Models.DonationsCenters;
using Hemo.Utilities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;

namespace Hemo.Controllers
{
    public class DonationsCentersController : ApiController
    {
        private IManager donationsCentersManager;
        private IDonationsCentersFactory donationsCentersFactory;

        public DonationsCentersController(IDonationsCentersManager donationsCentersManager, IDonationsCentersFactory donationsCentersFactory)
        {
            Guard.WhenArgument<IDonationsCentersManager>(donationsCentersManager, "Donations center manager cannot be null.")
                .IsNull()
                .Throw();
            Guard.WhenArgument<IDonationsCentersFactory>(donationsCentersFactory, "Donations centers factory cannot be null.")
                .IsNull()
                .Throw();

            this.donationsCentersManager = donationsCentersManager as IManager;
            this.donationsCentersFactory = donationsCentersFactory;
        }

        // GET api/centers/full
        [Authorize]
        [AcceptVerbs("GET")]
        [HttpGet]
        [Route("api/centers/full")]
        public HttpResponseMessage GetFullMedCenters(int skip = 0, int take = 0, decimal latitude = 0, decimal longitude = 0, bool inRange = false, string city = "", string country = "")
        {
            IList<DonationsCentersListViewModel> donationsCentersListViewModel = new List<DonationsCentersListViewModel>();

            IEnumerable<DonationsCenter> donationsCenters = this.donationsCentersManager.GetItems() as IEnumerable<DonationsCenter>;

            IEnumerable<DonationsCenter> query = donationsCenters;

            if (inRange)
            {
                query = query.Where(r => RadiusChecker.GetDistance((double)latitude, (double)longitude, (double)r.Latitude, (double)r.Longitude) < 25.0D);
            }

            if (!string.IsNullOrEmpty(city) && !string.IsNullOrEmpty(country))
            {
                query = query.Where(r => r.City.ToLower() == city.ToLower() && r.Country.ToLower() == country.ToLower());
            }

            query = query.Skip(skip).Take(take);


            foreach (DonationsCenter center in query)
            {
                donationsCentersListViewModel.Add(new DonationsCentersListViewModel()
                {
                    Id = center.Id,
                    Address = center.Address,
                    Latitude = center.Latitude,
                    Longitude = center.Longitude,
                    Name = center.Name,
                    Email = center.Email,
                    PhoneNumber = center.PhoneNumber,
                    Image = center.Image
                });
            }


            HttpResponseMessage resp = new HttpResponseMessage();

            resp.Content = new StringContent(JsonConvert.SerializeObject(donationsCentersListViewModel));
            resp.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            return resp;
        }

        // GET api/centers/{id}
        [Authorize]
        [AcceptVerbs("GET")]
        [HttpGet]
        [Route("api/centers/{id}")]
        public HttpResponseMessage GetCenters(Guid id)
        {
            DonationsCentersViewModel centerViewModel = new DonationsCentersViewModel();


            DonationsCenter center = this.donationsCentersManager.GetItem(id) as DonationsCenter;

            centerViewModel.Address = center.Address;
            centerViewModel.City = center.City;
            centerViewModel.Country = center.Country;
            centerViewModel.Latitude = center.Latitude;
            centerViewModel.Longitude = center.Longitude;
            centerViewModel.PhoneNumber = center.PhoneNumber;
            centerViewModel.Email = center.Email;
            centerViewModel.Name = center.Name;
            centerViewModel.Image = center.Image;

            HttpResponseMessage resp = new HttpResponseMessage();

            resp.Content = new StringContent(JsonConvert.SerializeObject(centerViewModel));
            resp.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            return resp;
        }
    }
}