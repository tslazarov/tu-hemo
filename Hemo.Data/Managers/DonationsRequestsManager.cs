using Bytes2you.Validation;
using Hemo.Data.Contracts;
using Hemo.Models;
using System;
using System.Collections.Generic;

namespace Hemo.Data.Managers
{
    public class DonationsRequestsManager : IManager
    {
        private readonly IData data;

        public DonationsRequestsManager(IData data)
        {
            Guard.WhenArgument<IData>(data, "Data cannot be null.")
                .IsNull()
                .Throw();

            this.data = data;
        }

        public IDataItem GetItem(Guid id)
        {
            return this.data.DonationsRequestsRepository.GetById(id);
        }

        public IEnumerable<IDataItem> GetItems()
        {
            return this.data.DonationsRequestsRepository.All();
        }

        public void CreateItem(IDataItem item)
        {
            this.data.DonationsRequestsRepository.Create((DonationsRequest)item);
        }

        public void DeleteItem(IDataItem item)
        {
            this.data.DonationsRequestsRepository.Delete((DonationsRequest)item);
        }

        public void UpdateItem(IDataItem item)
        {
            this.data.DonationsRequestsRepository.Update((DonationsRequest)item);
        }

        public void SaveChanges()
        {
            this.data.SaveChanges();
        }
    }
}
