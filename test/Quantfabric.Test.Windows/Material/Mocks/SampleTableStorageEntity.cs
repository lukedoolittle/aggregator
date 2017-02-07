using System.Runtime.Serialization;
using Material.Contracts;

namespace Quantfabric.Test.Material.Mocks
{
    [DataContract]
    public class SampleTableStorageEntity : ITableStorageEntity
    {
        [DataMember(Name = "Name")]
        public string Name { get; set; }

        public string PartitionKey { get; set; }
        public string RowKey { get; set; }
    }
}
