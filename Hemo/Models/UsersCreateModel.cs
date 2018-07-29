using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hemo.Models
{
    public class UsersCreateModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public bool IsExternal { get; set; }
        public string UserExternalId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public int Age { get; set; }
        public int BloodType { get; set; }
    }
}