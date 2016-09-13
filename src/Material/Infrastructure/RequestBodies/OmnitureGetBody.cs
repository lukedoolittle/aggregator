using System.Runtime.Serialization;

namespace Material.Infrastructure.RequestBodies
{
    [DataContract]
    public class OmnitureGetBody
    {
        [DataMember(Name = "reportID")]
        public string ReportId { get; set; }
    }
}
