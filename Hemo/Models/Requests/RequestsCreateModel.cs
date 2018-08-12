using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hemo.Models.Requests
{
    public class RequestsCreateModel
    {
        public string Address { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public int BloodQuantity { get; set; }
        public int RequestedBloodType { get; set; }
    }
}