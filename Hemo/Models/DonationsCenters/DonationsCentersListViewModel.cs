using Newtonsoft.Json;
using System;

namespace Hemo.Models.DonationsCenters
{
    public class DonationsCentersListViewModel
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("address")]
        public string Address { get; set; }

        [JsonProperty("latitude")]
        public decimal Latitude { get; set; }

        [JsonProperty("longitude")]
        public decimal Longitude { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("phoneNumber")]
        public string PhoneNumber { get; set; }

        [JsonProperty("image")]
        public string Image { get; set; }
    }
}