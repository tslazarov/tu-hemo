using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hemo.Models.Requests
{
    public class RequestListViewModel
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("bloodType")]
        public BloodType BloodType { get; set; }

        [JsonProperty("date")]
        public DateTime Date { get; set; }

        [JsonProperty("requestedBloodQuantity")]
        public int RequestedBloodQuantity { get; set; }
    }
}