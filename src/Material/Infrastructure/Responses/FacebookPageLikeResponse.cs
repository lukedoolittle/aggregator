using System.Runtime.Serialization;
using System;
using System.Collections.Generic;
using Foundations.HttpClient.Metadata;
namespace Material.Infrastructure.Responses
{
    [DataContract]
    public class FacebookLikeDatum
    {
        [DataMember(Name = "name")]
        public string Name { get; set; }
        [DataMember(Name = "id")]
        public string Id { get; set; }
        [DataMember(Name = "created_time")]
        public DateTime CreatedTime { get; set; }
    }

    [DataContract]
    public class FacebookCursors
    {
        [DataMember(Name = "before")]
        public string Before { get; set; }
        [DataMember(Name = "after")]
        public string After { get; set; }
    }

    [DataContract]
    public class FacebookPaging
    {
        [DataMember(Name = "cursors")]
        public FacebookCursors Cursors { get; set; }
        [DataMember(Name = "next")]
        public string Next { get; set; }
    }

    [DateTimeFormatter("yyyy-MM-ddTHH:mm:sszzz")]
    [DataContract]
    public class FacebookPageLikeResponse
    {
        [DataMember(Name = "data")]
        public IList<FacebookLikeDatum> Data { get; set; }
        [DataMember(Name = "paging")]
        public FacebookPaging Paging { get; set; }
    }


}
