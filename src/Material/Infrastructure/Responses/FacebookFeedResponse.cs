using System.Runtime.Serialization;
using System;
using System.Collections.Generic;
using Foundations.HttpClient.Metadata;

namespace Material.Infrastructure.Responses
{
    [DataContract]
    public class FacebookFeedDatum
    {
        [DataMember(Name = "message")]
        public string Message { get; set; }
        [DataMember(Name = "created_time")]
        public DateTime CreatedTime { get; set; }
        [DataMember(Name = "id")]
        public string Id { get; set; }
        [DataMember(Name = "story")]
        public string Story { get; set; }
    }

    [DataContract]
    public class FacebookFeedPaging
    {
        [DataMember(Name = "previous")]
        public string Previous { get; set; }
        [DataMember(Name = "next")]
        public string Next { get; set; }
    }

    [DatetimeFormatter("yyyy-MM-ddTHH:mm:sszzz")]
    [DataContract]
    public class FacebookFeedResponse
    {
        [DataMember(Name = "data")]
        public IList<FacebookFeedDatum> Data { get; set; }
        [DataMember(Name = "paging")]
        public FacebookFeedPaging Paging { get; set; }
    }


}
