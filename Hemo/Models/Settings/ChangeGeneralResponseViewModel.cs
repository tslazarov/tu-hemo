using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hemo.Models.Settings
{
    public class ChangeGeneralResponseViewModel
    {
        [JsonProperty("isSuccessful")]
        public bool IsSuccessful { get; set; }

        [JsonProperty("state")]
        public string State { get; set; }
    }
}