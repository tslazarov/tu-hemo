using Newtonsoft.Json;
using System;

namespace Hemo.Models.Requests
{
    public class RequestUserListViewModel
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("bloodType")]
        public BloodType BloodType { get; set; }

        [JsonProperty("date")]
        public DateTime Date { get; set; }

        [JsonProperty("requestedBloodQuantity")]
        public int RequestedBloodQuantity { get; set; }

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