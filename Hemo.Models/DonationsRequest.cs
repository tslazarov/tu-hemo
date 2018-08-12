using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Hemo.Models
{
    public class DonationsRequest : IDataItem
    {
        private ICollection<Donator> donators;

        public DonationsRequest()
        {
            this.Donators = new List<Donator>();
        }

        public DonationsRequest(Guid id, Guid ownerId, string address, string city, string country, decimal latitude, decimal longitude, BloodType requestedBloodType, int requestedBloodQuantityInMl)
        {
            this.Id = id;
            this.OwnerId = ownerId;
            this.Address = address;
            this.City = city;
            this.Country = country;
            this.Latitude = latitude;
            this.Longitude = longitude;
            this.RequestedBloodType = requestedBloodType;
            this.RequestedBloodQuantityInMl = requestedBloodQuantityInMl;
            this.Donators = new List<Donator>();
        }

        [Key]
        public Guid Id { get; set; }

        [Required]
        public Guid OwnerId { get; set; }

        [Required]
        public string Address { get; set; }

        [Required]
        public string City { get; set; }

        [Required]
        public string Country { get; set; }

        [Required]
        [DataType("decimal(9,6)")]
        public decimal Longitude { get; set; }

        [Required]
        [DataType("decimal(9,6)")]
        public decimal Latitude { get; set; }

        public ICollection<Donator> Donators
        {
            get
            {
                return this.donators;
            }
            set
            {
                this.donators = value;
            }
        }

        [Required]
        public BloodType RequestedBloodType { get; set; }

        [Required]
        [Range(0, 50000)]
        public int RequestedBloodQuantityInMl { get; set; }
    }
}
