using Hemo.Models;
using System;

namespace Hemo.Data.Factories
{
    public interface IDonationsCentersFactory
    {
        DonationsCenter Create(Guid id, string name, string address, string city, string country, string phoneNumber, decimal latitude, decimal longitude);
    }
}
