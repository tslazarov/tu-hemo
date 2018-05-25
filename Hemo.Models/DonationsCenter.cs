using System;
using System.ComponentModel.DataAnnotations;

namespace Hemo.Models
{
    public class DonationsCenter
    {
        public DonationsCenter()
        {
        }

        public DonationsCenter(Guid id, string address, string phoneNumber, decimal longtitude, decimal latitude)
        {
            this.Id = id;
            this.Address = address;
            this.PhoneNumber = phoneNumber;
            this.Longtitude = Longtitude;
            this.Latitude = latitude;
        }

        [Key]
        public Guid Id { get; set; }

        [Required]
        public string Address { get; set; }

        [Required]
        public string PhoneNumber { get; set; }

        public string EmailAddress { get; set; }

        [Required]
        [DataType("decimal(9,6)")]
        public decimal Longtitude { get; set; }

        [Required]
        [DataType("decimal(9,6)")]
        public decimal Latitude { get; set; }
    }
}
