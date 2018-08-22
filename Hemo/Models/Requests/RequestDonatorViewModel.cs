using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hemo.Models.Requests
{
    public class RequestDonatorViewModel
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("bloodType")]
        public BloodType BloodType { get; set; }

        [JsonProperty("isApproved")]
        public bool IsApproved { get; set; }
    }
}