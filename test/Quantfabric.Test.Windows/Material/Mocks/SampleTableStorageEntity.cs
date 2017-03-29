using System;
using System.Runtime.Serialization;
using Material.Contracts;
using Material.Framework.Metadata;

namespace Quantfabric.Test.Material.Mocks
{
    [DataContract]
    [ClassDateTimeFormatter("o")]
    public class SampleTableStorageEntity : TableStorageEntity
    {
        [DataMember(Name = "Name")]
        public string Name { get; set; }

        [DataMember(Name = "MyTime")]
        public DateTime Timestamp { get; set; }

        public SampleTableStorageEntity(
            string partitionKey, 
            string rowKey) : 
                base(
                    partitionKey, 
                    rowKey)
        {}
    }
}
