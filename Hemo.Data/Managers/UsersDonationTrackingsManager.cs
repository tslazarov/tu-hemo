using Bytes2you.Validation;
using Hemo.Data.Contracts;
using Hemo.Models;
using System;
using System.Collections.Generic;

namespace Hemo.Data.Managers
{
    public class UsersDonationTrackingsManager : IUsersDonationTrackingsManager, IManager
    {
        private readonly IData data;

        public UsersDonationTrackingsManager(IData data)
        {
            Guard.WhenArgument<IData>(data, "Data cannot be null.")
                .IsNull()
                .Throw();

            this.data = data;
        }

        public IDataItem GetItem(Guid id)
        {
            return this.data.UsersDonationTrackingsRepository.GetById(id);
        }

        public IEnumerable<IDataItem> GetItems()
        {
            return this.data.UsersDonationTrackingsRepository.All();
        }

        public void CreateItem(IDataItem item)
        {
            this.data.UsersDonationTrackingsRepository.Create((UsersDonationTracking)item);
        }

        public void DeleteItem(IDataItem item)
        {
            this.data.UsersDonationTrackingsRepository.Delete((UsersDonationTracking)item);
        }

        public void UpdateItem(IDataItem item)
        {
            this.data.UsersDonationTrackingsRepository.Update((UsersDonationTracking)item);
        }

        public void SaveChanges()
        {
            this.data.SaveChanges();
        }
    }
}
