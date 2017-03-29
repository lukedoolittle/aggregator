using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Material.Domain.Responses
{
    [GeneratedCode("T4Toolbox", "14.0")]
    [DataContract]
    public class XamarinEventHit
    {
        [DataMember(Name = "deviceTimestamp")]
        public int DeviceTimestamp { get; set; }

        [DataMember(Name = "properties")]
        public IDictionary<string,string> Properties { get; set; }

        [DataMember(Name = "deviceTimestampMs")]
        public object DeviceTimestampMs { get; set; }

        [DataMember(Name = "order")]
        public int Order { get; set; }

        [DataMember(Name = "duration")]
        public string Duration { get; set; }

        [DataMember(Name = "trackevent")]
        public int Trackevent { get; set; }

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
    public class XamarinEventHits
    {
        [DataMember(Name = "hits")]
        public IList<XamarinEventHit> Hits { get; set; }
    }

    [GeneratedCode("T4Toolbox", "14.0")]
    [DataContract]
    public class XamarinEventResponse
    {
        [DataMember(Name = "hits")]
        public XamarinEventHits Hits { get; set; }
    }
}
