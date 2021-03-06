﻿using System;

namespace Hemo.Models.Requests
{
    public class RequestsEditModel
    {
        public string Address { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public int BloodQuantity { get; set; }
        public int RequestedBloodType { get; set; }
        public Guid Id { get; set; }
    }
}