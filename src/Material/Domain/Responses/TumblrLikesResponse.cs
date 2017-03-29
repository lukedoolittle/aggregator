using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Material.Domain.Responses
{
    [GeneratedCode("T4Toolbox", "14.0")]
    [DataContract]
    public class Meta
    {

        [DataMember(Name = "status")]
        public int Status { get; set; }

        [DataMember(Name = "msg")]
        public string Msg { get; set; }
    }

    [GeneratedCode("T4Toolbox", "14.0")]
    [DataContract]
    public class Response
    {

        [DataMember(Name = "liked_posts")]
        public IList<object> LikedPosts { get; set; }

        [DataMember(Name = "liked_count")]
        public int LikedCount { get; set; }
    }

    [GeneratedCode("T4Toolbox", "14.0")]
    [DataContract]
    public class TumblrLikeResponse
    {

        [DataMember(Name = "meta")]
        public Meta Meta { get; set; }

        [DataMember(Name = "response")]
        public Response Response { get; set; }
    }
}
