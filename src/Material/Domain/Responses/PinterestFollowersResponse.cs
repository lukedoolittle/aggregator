using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Material.Domain.Responses
{
    [GeneratedCode("T4Toolbox", "14.0")]
    [DataContract]
    public class PinterestFollowersResponse
    {
        [DataMember(Name = "data")]
        public IList<PinterestUserDatum> Data { get; set; }

        [DataMember(Name = "page")]
        public PinterestPage Page { get; set; }
    }
}
