using Hemo.Models.Requests;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Hemo.Models.Trackings
{
    public class TrackingsViewModel
    {
        [JsonProperty("latestRequestDonation")]
        public RequestListViewModel LatestRequestDonation { get; set; }

        [JsonProperty("locations")]
        public Dictionary<string, int> Locations { get; set; }

        [JsonProperty("annualDonations")]
        public Dictionary<int, int> AnnualDonations { get; set; }
    }
}