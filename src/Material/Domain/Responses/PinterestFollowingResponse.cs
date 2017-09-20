using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Material.Domain.Responses
{
    [GeneratedCode("T4Toolbox", "14.0")]
    [DataContract]
    public class PinterestPage
    {
        [DataMember(Name = "cursor")]
        public object Cursor { get; set; }

        [DataMember(Name = "next")]
        public object Next { get; set; }
    }

    [GeneratedCode("T4Toolbox", "14.0")]
    [DataContract]
    public class PinterestFollowingResponse
    {
        [DataMember(Name = "data")]
        public IList<PinterestUserDatum> Data { get; set; }

        [DataMember(Name = "page")]
        public PinterestPage Page { get; set; }
    }
}
