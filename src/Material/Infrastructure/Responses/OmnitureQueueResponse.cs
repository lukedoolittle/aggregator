using System.Runtime.Serialization;

namespace Material.Infrastructure.Responses
{
    [DataContract]
    public class OmnitureQueueResponse
    {
        [DataMember(Name = "reportID")]
        public string ReportId { get; set; }
    }
}
