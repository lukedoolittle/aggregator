using System.Runtime.Serialization;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using Foundations.HttpClient.Metadata;
namespace Material.Infrastructure.Responses
{
    [DataContract]
    public class FacebookLikeDatum
    {
        [DataMember(Name = "name")]
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }
        [DataMember(Name = "id")]
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }
        [DataMember(Name = "created_time")]
        [JsonProperty(PropertyName = "created_time")]
        public DateTime CreatedTime { get; set; }
    }

    [DataContract]
    public class FacebookCursors
    {
        [DataMember(Name = "before")]
        [JsonProperty(PropertyName = "before")]
        public string Before { get; set; }
        [DataMember(Name = "after")]
        [JsonProperty(PropertyName = "after")]
        public string After { get; set; }
    }

    [DataContract]
    public class FacebookPaging
    {
        [DataMember(Name = "cursors")]
        [JsonProperty(PropertyName = "cursors")]
        public FacebookCursors Cursors { get; set; }
        [DataMember(Name = "next")]
        [JsonProperty(PropertyName = "next")]
        public string Next { get; set; }
    }

    [DatetimeFormatter("yyyy-MM-ddTHH:mm:sszzz")]
    [DataContract]
    public class FacebookPageLikeResponse
    {
        [DataMember(Name = "data")]
        [JsonProperty(PropertyName = "data")]
        public IList<FacebookLikeDatum> Data { get; set; }
        [DataMember(Name = "paging")]
        [JsonProperty(PropertyName = "paging")]
        public FacebookPaging Paging { get; set; }
    }


}
