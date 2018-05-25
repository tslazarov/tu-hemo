using Hemo.Models;
using System;

namespace Hemo.Data.Factories
{
    public interface IUsersDonationTrackingsFactory
    {
        UsersDonationTracking Create(Guid id);
    }
}
