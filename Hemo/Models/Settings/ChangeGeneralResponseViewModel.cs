using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hemo.Models.Settings
{
    public class ChangeGeneralResponseViewModel
    {
        [JsonProperty("isChanged")]
        public bool IsChanged { get; set; }

        [JsonProperty("state")]
        public string State { get; set; }
    }
}