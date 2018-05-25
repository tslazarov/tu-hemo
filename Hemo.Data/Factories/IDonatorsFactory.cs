using Hemo.Models;
using System;

namespace Hemo.Data.Factories
{
    public interface IDonatorsFactory
    {
        Donator Create(Guid id, string email, bool isApproved = false);
    }
}
