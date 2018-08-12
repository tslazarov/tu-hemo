using Hemo.Models;
using System;

namespace Hemo.Data.Factories
{
    public interface IDonationsRequestsFactory
    {
        DonationsRequest Create(Guid id, Guid ownerId, string address, string city, string country, decimal latitude, decimal longitude, BloodType requestedBloodType, int requestedBloodQuantityInMl);
    }
}
