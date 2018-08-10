using Newtonsoft.Json;

namespace Hemo.Models.Users
{
    public class PreferredLanguageViewModel
    {
        [JsonProperty("preferredLanguage")]
        public int PreferredLanguage { get; set; }
    }
}