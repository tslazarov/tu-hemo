using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hemo.Models.Requests
{
    public class RequestViewModel
    {
        [JsonProperty("isSigned")]
        public bool IsSigned { get; set; }

        [JsonProperty("owner")]
        public RequestOwnerViewModel Owner { get; set; }

        [JsonProperty("address")]
        public string Address { get; set; }

        [JsonProperty("city")]
        public string City { get; set; }

        [JsonProperty("country")]
        public string Country { get; set; }

        [JsonProperty("date")]
        public DateTime Date { get; set; }

        [JsonProperty("longitude")]
        public decimal Longitude { get; set; }

        [JsonProperty("latitude")]
        public decimal Latitude { get; set; }

        [JsonProperty("requestedBloodType")]
        public BloodType RequestedBloodType { get; set; }

        [JsonProperty("requestedBloodQuantity")]
        public int RequestedBloodQuantityInMl { get; set; }

        [JsonProperty("donators")]
        public IList<RequestDonatorViewModel> Donators { get; set; }
    }
}