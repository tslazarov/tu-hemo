using System;
using System.ComponentModel.DataAnnotations;

namespace Hemo.Models
{
    public class DonationsCenter : IDataItem
    {
        public DonationsCenter()
        {
        }

        public DonationsCenter(Guid id, string address, string city, string country, string phoneNumber, decimal latitude, decimal longitude)
        {
            this.Id = id;
            this.Address = address;
            this.City = address;
            this.Country = address;
            this.PhoneNumber = phoneNumber;
            this.Latitude = latitude;
            this.Longitude = longitude;
        }

        [Key]
        public Guid Id { get; set; }

        [Required]
        public string Address { get; set; }

        [Required]
        public string City { get; set; }

        [Required]
        public string Country { get; set; }

        [Required]
        public string PhoneNumber { get; set; }

        public string EmailAddress { get; set; }

        [Required]
        [DataType("decimal(9,6)")]
        public decimal Longitude { get; set; }

        [Required]
        [DataType("decimal(9,6)")]
        public decimal Latitude { get; set; }
    }
}
