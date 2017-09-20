using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Material.Domain.Responses
{
    [GeneratedCode("T4Toolbox", "14.0")]
    [DataContract]
    public class PinterestPinDatum
    {
        [DataMember(Name = "url")]
        public string Url { get; set; }

        [DataMember(Name = "note")]
        public string Note { get; set; }

        [DataMember(Name = "link")]
        public string Link { get; set; }

        [DataMember(Name = "id")]
        public string Id { get; set; }
    }

    [GeneratedCode("T4Toolbox", "14.0")]
    [DataContract]
    public class PinterestPinsResponse
    {
        [DataMember(Name = "data")]
        public IList<PinterestPinDatum> Data { get; set; }

        [DataMember(Name = "page")]
        public PinterestPage Page { get; set; }
    }
}
