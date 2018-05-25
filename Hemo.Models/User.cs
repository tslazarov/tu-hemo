using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hemo.Models
{
    public class User
    {
        private ICollection<DonationsRequest> donationsRequests;

        public User()
        {
        }

        public User(Guid id, string email, string firstName, string lastName)
        {
            this.Id = id;
            this.Email = email;
            this.FirstName = firstName;
            this.LastName = lastName;
            this.DonationsRequests = new List<DonationsRequest>();
        }

        [Key]
        public Guid Id { get; set; }

        [Required]
        [MinLength(3)]
        [MaxLength(40)]
        [Index(IsUnique = true)]
        [EmailAddress]
        public string Email { get; set; }

        [MinLength(3)]
        [MaxLength(30)]
        public string FirstName { get; set; }

        [MinLength(3)]
        [MaxLength(30)]
        public string LastName { get; set; }

        [Required]
        public string Salt { get; set; }

        public int Age { get; set; }

        [Required]
        public string HashedPassword { get; set; }

        public BloodType BloodType { get; set; }

        public string PhoneNumber { get; set; }

        public string Image { get; set; }

        public Guid TrackingId { get; set; }

        public virtual UsersDonationTracking Tracking { get; set; }

        public virtual ICollection<DonationsRequest> DonationsRequests
        {
            get
            {
                return this.donationsRequests;
            }
            set
            {
                this.donationsRequests = value;
            }
        }
    }
}
