using System;
using System.Runtime.Serialization;
using Foundations.HttpClient.Metadata;
using Material.Contracts;

namespace Quantfabric.Test.Material.Mocks
{
    [DataContract]
    [DateTimeFormatter("o")]
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
