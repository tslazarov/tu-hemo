using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hemo.Models.Users
{
    public class UsersResetPasswordModel
    {
        public string Email { get; set; }
        public string ResetCode { get; set; }
        public string NewPassword { get; set; }
    }
}