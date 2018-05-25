using Hemo.Models;
using System;
using System.Collections.Generic;

namespace Hemo.Data.Contracts
{
    public interface IManager
    {
        IDataItem GetItem(Guid id);

        IEnumerable<IDataItem> GetItems();

        void CreateItem(IDataItem item);

        void UpdateItem(IDataItem item);

        void DeleteItem(IDataItem item);

        void SaveChanges();
    }
}
