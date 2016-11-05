using System.CodeDom.Compiler;
using System.Runtime.Serialization;

namespace Material.Infrastructure.Responses
{
    [GeneratedCode("T4Toolbox", "14.0")]
    [DataContract]
    public class OmnitureQueueResponse
    {
        [DataMember(Name = "reportID")]
        public string ReportId { get; set; }
    }
}
