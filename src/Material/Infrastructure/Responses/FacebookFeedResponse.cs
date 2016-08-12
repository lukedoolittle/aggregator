using System.Runtime.Serialization;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using Foundations.HttpClient.Metadata;

namespace Material.Infrastructure.Responses
{
    [DataContract]
    public class FacebookFeedDatum
    {
        [DataMember(Name = "message")]
        [JsonProperty(PropertyName = "message")]
        public string Message { get; set; }
        [DataMember(Name = "created_time")]
        [JsonProperty(PropertyName = "created_time")]
        public DateTime CreatedTime { get; set; }
        [DataMember(Name = "id")]
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }
        [DataMember(Name = "story")]
        [JsonProperty(PropertyName = "story")]
        public string Story { get; set; }
    }

    [DataContract]
    public class FacebookFeedPaging
    {
        [DataMember(Name = "previous")]
        [JsonProperty(PropertyName = "previous")]
        public string Previous { get; set; }
        [DataMember(Name = "next")]
        [JsonProperty(PropertyName = "next")]
        public string Next { get; set; }
    }

    [DatetimeFormatter("yyyy-MM-ddTHH:mm:sszzz")]
    [DataContract]
    public class FacebookFeedResponse
    {
        [DataMember(Name = "data")]
        [JsonProperty(PropertyName = "data")]
        public IList<FacebookFeedDatum> Data { get; set; }
        [DataMember(Name = "paging")]
        [JsonProperty(PropertyName = "paging")]
        public FacebookFeedPaging Paging { get; set; }
    }


}
