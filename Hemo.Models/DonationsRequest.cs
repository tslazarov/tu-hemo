using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Hemo.Models
{
    public class DonationsRequest
    {
        private ICollection<Donator> donators;

        public DonationsRequest()
        {
            this.Donators = new List<Donator>();
        }

        public DonationsRequest(Guid id, Guid ownerId, string address, decimal longtitude, decimal latitude, BloodType requestedBloodType, int requestedBloodQuantityInMl)
        {
            this.Id = id;
            this.OwnerId = ownerId;
            this.Address = address;
            this.Longtitude = longtitude;
            this.Latitude = latitude;
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
        [DataType("decimal(9,6)")]
        public decimal Longtitude { get; set; }

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
