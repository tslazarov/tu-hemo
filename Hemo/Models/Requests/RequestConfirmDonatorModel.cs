using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hemo.Models.Requests
{
    public class RequestConfirmDonatorModel
    {
        public Guid UserId { get; set; }

        public Guid RequestId { get; set; }
    }
}