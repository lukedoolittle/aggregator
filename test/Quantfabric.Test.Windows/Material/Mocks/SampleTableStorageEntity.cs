using System.Runtime.Serialization;
using Material.Contracts;

namespace Quantfabric.Test.Material.Mocks
{
    [DataContract]
    public class SampleTableStorageEntity : TableStorageEntity
    {
        [DataMember(Name = "Name")]
        public string Name { get; set; }


        public SampleTableStorageEntity(
            string partitionKey, 
            string rowKey) : 
                base(
                    partitionKey, 
                    rowKey)
        {}
    }
}
