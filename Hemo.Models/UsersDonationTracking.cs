using System;
using System.ComponentModel.DataAnnotations;

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
        public Guid Id { get; set; }

        public DateTime? LastDonation { get; set; }

        public int CurrentAnnualDonations { get; set; }

        public int MaxAnnualDonations { get; set; }
    }
}
