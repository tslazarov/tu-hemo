using Newtonsoft.Json;

namespace Hemo.Models.Users
{
    public class UsersBasicProfileViewModel
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("profileImage")]
        public string ProfileImage { get; set; }
    }
}