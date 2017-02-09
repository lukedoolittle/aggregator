using System;
using Material.Contracts;

namespace Material.Infrastructure.Requests
{
    public partial class AzureTableStorageNonQuery
    {
        private string _storageAccountName;

        public AzureTableStorageNonQuery SetAccount(
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

        public AzureTableStorageNonQuery SetBody<TEntity>(TEntity entity)
            where TEntity : ITableStorageEntity
        {
            PartitionKey = entity.PartitionKey;
            RowKey = entity.RowKey;

            Body = entity;

            return this;
        }
    }
}
