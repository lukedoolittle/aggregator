using System.CodeDom.Compiler;
using System.Runtime.Serialization;

namespace Material.Infrastructure.RequestBodies
{
    [GeneratedCode("T4Toolbox", "14.0")]
    [DataContract]
    public class OmnitureGetBody
    {
        [DataMember(Name = "reportID")]
        public string ReportId { get; set; }
    }
}
