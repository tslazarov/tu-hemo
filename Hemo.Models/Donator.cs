using System;
using System.ComponentModel.DataAnnotations;

namespace Hemo.Models
{
    public class Donator : IDataItem
    {
        public Donator()
        {
        }

        public Donator(Guid id, Guid userId, bool isApproved = false)
        {
            this.Id = id;
            this.UserId = userId;
            this.IsApproved = isApproved;
        }

        [Key]
        public Guid Id { get; set; }

        public Guid UserId { get; set; }

        public User User { get; set; }

        public bool IsApproved { get; set; }
    }
}