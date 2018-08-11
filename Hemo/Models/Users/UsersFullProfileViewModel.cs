using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hemo.Models.Users
{
    public class UsersFullProfileViewModel
    {
        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("firstName")]
        public string FirstName { get; set; }

        [JsonProperty("lastName")]
        public string LastName { get; set; }

        [JsonProperty("phoneNumber")]
        public string PhoneNumber { get; set; }

        [JsonProperty("age")]
        public int Age { get; set; }

        [JsonProperty("bloodType")]
        public int BloodType { get; set; }

        [JsonProperty("profileImage")]
        public string ProfileImage { get; set; }
    }
}