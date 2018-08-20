using Hemo.Models;
using System;

namespace Hemo.Data.Factories
{
    public interface IDonatorsFactory
    {
        Donator Create(Guid id, Guid userId, User user, bool isApproved = false);
    }
}
