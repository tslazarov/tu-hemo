﻿using Hemo.Models;
using System;

namespace Hemo.Data.Factories
{
    public interface IUsersFactory
    {
        User Create(Guid id, string email, string firstName, string lastName, bool isExternal);
    }
}
