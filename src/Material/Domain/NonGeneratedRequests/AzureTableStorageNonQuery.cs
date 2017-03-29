using System;
using Material.Contracts;

namespace Material.Domain.Requests
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
            if (entity == null) throw new ArgumentNullException(nameof(entity));

            PartitionKey = entity.PartitionKey;
            RowKey = entity.RowKey;

            AddContent(entity);

            return this;
        }
    }
}
