using System;

namespace Hemo.Models.Requests
{
    public class RequestDisconfirmDonatorModel
    {
        public Guid UserId { get; set; }

        public Guid RequestId { get; set; }
    }
}