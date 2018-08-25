using Hemo.Models.Requests;
using Newtonsoft.Json;
using System;

namespace Hemo.Models.Trackings
{
    public class TrackingsViewModel
    {
        [JsonProperty("lastDonation")]
        public DateTime? LastDonation { get; set; }

        [JsonProperty("currentAnnualDonations")]
        public int CurrentAnnualDonations { get; set; }

        [JsonProperty("lastRequestDonation")]
        public RequestListViewModel LastRequestDonation { get; set; }
    }
}