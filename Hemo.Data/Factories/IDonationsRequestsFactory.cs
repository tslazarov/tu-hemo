using Hemo.Models;
using System;

namespace Hemo.Data.Factories
{
    public interface IDonationsRequestsFactory
    {
        DonationsRequest Create(Guid id, Guid ownerId, string address, decimal longtitude, decimal latitude, BloodType requestedBloodType, int requestedBloodQuantityInMl);
    }
}
