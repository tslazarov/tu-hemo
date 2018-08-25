using Newtonsoft.Json;

namespace Hemo.Models.Requests
{
    public class RequestOwnerViewModel
    {
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