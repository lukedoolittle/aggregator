using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Material.Domain.Responses
{
    [GeneratedCode("T4Toolbox", "14.0")]
    [DataContract]
    public class XamarinSessionHit
    {
        [DataMember(Name = "deviceTimestamp")]
        public int DeviceTimestamp { get; set; }

        [DataMember(Name = "order")]
        public int Order { get; set; }

        [DataMember(Name = "trackevent")]
        public int Trackevent { get; set; }

        [DataMember(Name = "deviceTimestampMs")]
        public object DeviceTimestampMs { get; set; }

        [DataMember(Name = "state")]
        public int State { get; set; }

        [DataMember(Name = "type")]
        public int Type { get; set; }

        [DataMember(Name = "id")]
        public string Id { get; set; }

        [DataMember(Name = "trackId")]
        public string TrackId { get; set; }
    }

    [GeneratedCode("T4Toolbox", "14.0")]
    [DataContract]
    public class XamarinSessionHits
    {
        [DataMember(Name = "hits")]
        public IList<XamarinSessionHit> Hits { get; set; }
    }

    [GeneratedCode("T4Toolbox", "14.0")]
    [DataContract]
    public class XamarinSessionResponse
    {
        [DataMember(Name = "hits")]
        public XamarinSessionHits Hits { get; set; }
    }
}
