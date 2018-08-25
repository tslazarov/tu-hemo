using System;

namespace Hemo.Models.Requests
{
    public class RequestConfirmDonatorModel
    {
        public Guid UserId { get; set; }

        public Guid RequestId { get; set; }
    }
}