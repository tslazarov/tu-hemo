using Newtonsoft.Json;

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