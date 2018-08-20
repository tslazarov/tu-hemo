using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hemo.Models
{
    public class UsersDonationTracking : IDataItem
    {
        public UsersDonationTracking()
        {
        }

        public UsersDonationTracking(Guid id)
        {
            this.Id = id;
            this.MaxAnnualDonations = 4;
        }

        [Key]
        [ForeignKey("User")]
        public Guid Id { get; set; }

        public DateTime? LastDonation { get; set; }

        public int CurrentAnnualDonations { get; set; }

        public int MaxAnnualDonations { get; set; }

        public Guid UserId { get; set; }

        public virtual User User { get; set; }
    }
}
