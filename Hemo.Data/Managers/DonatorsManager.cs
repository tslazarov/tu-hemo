using Bytes2you.Validation;
using Hemo.Data.Contracts;
using Hemo.Models;
using System;
using System.Collections.Generic;

namespace Hemo.Data.Managers
{
    public class DonatorsManager : IDonatorsManager, IManager
    {
        private readonly IData data;

        public DonatorsManager(IData data)
        {
            Guard.WhenArgument<IData>(data, "Data cannot be null.")
                .IsNull()
                .Throw();

            this.data = data;
        }

        public IDataItem GetItem(Guid id)
        {
            return this.data.DonatorsRepository.GetById(id);
        }

        public IEnumerable<IDataItem> GetItems()
        {
            return this.data.DonatorsRepository.All();
        }

        public void CreateItem(IDataItem item)
        {
            this.data.DonatorsRepository.Create((Donator)item);
        }

        public void DeleteItem(IDataItem item)
        {
            this.data.DonatorsRepository.Delete((Donator)item);
        }

        public void UpdateItem(IDataItem item)
        {
            this.data.DonatorsRepository.Update((Donator)item);
        }

        public void SaveChanges()
        {
            this.data.SaveChanges();
        }
    }
}
