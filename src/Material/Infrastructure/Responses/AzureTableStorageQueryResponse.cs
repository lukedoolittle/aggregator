using System.Collections.Generic;
using System.Runtime.Serialization;
using Foundations.HttpClient.Metadata;
using Material.Contracts;

namespace Material.Infrastructure.Responses
{
    [DataContract]
    [DateTimeFormatter("o")]
    public class AzureTableStorageQueryResponse<TEntity>
        where TEntity : ITableStorageEntity
    {
        [DataMember(Name = "odata.metadata")]
        public string ODataMetadata { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        [DataMember(Name = "value")]
        public IList<TEntity> Value { get; set; }
    }
}
