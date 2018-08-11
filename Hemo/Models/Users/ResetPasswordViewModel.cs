using Newtonsoft.Json;

namespace Hemo.Models.Users
{
    public class ResetPasswordViewModel
    {
        [JsonProperty("resetCode")]
        public string ResetCode { get; set; }
    }
}