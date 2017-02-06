using System;
using System.Runtime.Serialization;
using Material.Contracts;

namespace Quantfabric.Test.Material.Mocks
{
    public class SampleTableStorageEntity : ITableStorageEntity
    {
        public string PartitionKey { get; set; }
        public string RowKey { get; set; }
        public DateTime Timestamp { get; set; }

        [DataMember(Name = "Name")]
        public string Name { get; set; }
    }
}
