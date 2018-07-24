using Bytes2you.Validation;
using Hemo.Data.Contracts;
using Hemo.Models;
using System;
using System.Collections.Generic;

namespace Hemo.Data.Managers
{
    public class DonationsCentersManager : IDonationsCentersManager, IManager
    {
        private readonly IData data;

        public DonationsCentersManager(IData data)
        {
            Guard.WhenArgument<IData>(data, "Data cannot be null.")
                .IsNull()
                .Throw();

            this.data = data;
        }


        public IDataItem GetItem(Guid id)
        {
            return this.data.DonationsCentersRepository.GetById(id);
        }

        public IEnumerable<IDataItem> GetItems()
        {
            return this.data.DonationsCentersRepository.All();
        }

        public void CreateItem(IDataItem item)
        {
            this.data.DonationsCentersRepository.Create((DonationsCenter)item);
        }

        public void DeleteItem(IDataItem item)
        {
            this.data.DonationsCentersRepository.Delete((DonationsCenter)item);
        }

        public void UpdateItem(IDataItem item)
        {
            this.data.DonationsCentersRepository.Update((DonationsCenter)item);
        }

        public void SaveChanges()
        {
            this.data.SaveChanges();
        }
    }
}
