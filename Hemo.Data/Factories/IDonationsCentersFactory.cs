using Hemo.Models;
using System;

namespace Hemo.Data.Factories
{
    public interface IDonationsCentersFactory
    {
        DonationsCenter Create(Guid id, string address, string phoneNumber, decimal longtitude, decimal latitude);
    }
}
