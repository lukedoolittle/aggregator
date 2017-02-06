using System;
using System.Runtime.Serialization;

namespace Material.Contracts
{
    public interface ITableStorageEntity
    {
        [DataMember(Name = "PartitionKey")]
        string PartitionKey { get; set; }

        [DataMember(Name = "RowKey")]
        string RowKey { get; set; }

        [DataMember(Name = "Timestamp")]
        DateTime Timestamp { get; set; }
    }
}
