using System;
using System.Collections.Generic;
using Material.Contracts;

namespace Material.Domain.Requests
{
    public partial class AzureTableStorageBatch
    {
        private string _storageAccountName;

        public AzureTableStorageBatch SetAccount(
            string accountName)
        {
            if (accountName == null) throw new ArgumentNullException(nameof(accountName));

            _storageAccountName = accountName;

            return this;
        }

        public override string GetModifiedHost()
        {
            if (_storageAccountName == null)
            {
                throw new ArgumentNullException(nameof(_storageAccountName));
            }

            return Host.Replace("ACCOUNTNAME", _storageAccountName);
        }

        public AzureTableStorageBatch SetBody<TEntity>(IEnumerable<TEntity> entities)
            where TEntity : ITableStorageEntity
        {
            if (entities == null) throw new ArgumentNullException(nameof(entities));

            foreach (var entity in entities)
            {
                AddContent(entity);
            }

            return this;
        }
    }
}
