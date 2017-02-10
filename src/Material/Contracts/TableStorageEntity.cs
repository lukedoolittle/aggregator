using System.Runtime.Serialization;

namespace Material.Contracts
{
    [DataContract]
    public abstract class TableStorageEntity : ITableStorageEntity
    {
        [DataMember(Name = "PartitionKey")]
        public string PartitionKey { get; set; }

        [DataMember(Name = "RowKey")]
        public string RowKey { get; set; }

        protected TableStorageEntity(
            string partitionKey, 
            string rowKey)
        {
            PartitionKey = partitionKey;
            RowKey = rowKey;
        }
    }
}
