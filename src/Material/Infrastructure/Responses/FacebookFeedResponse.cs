using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Material.Infrastructure.Responses
{
    public class FacebookFeedDatum
    {
        [JsonProperty(PropertyName = "message")]
        public string Message { get; set; }
        [JsonProperty(PropertyName = "created_time")]
        public DateTime CreatedTime { get; set; }
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }
        [JsonProperty(PropertyName = "story")]
        public string Story { get; set; }
    }

    public class FacebookFeedPaging
    {
        [JsonProperty(PropertyName = "previous")]
        public string Previous { get; set; }
        [JsonProperty(PropertyName = "next")]
        public string Next { get; set; }
    }

    public class FacebookFeedResponse
    {
        [JsonProperty(PropertyName = "data")]
        public IList<FacebookFeedDatum> Data { get; set; }
        [JsonProperty(PropertyName = "paging")]
        public FacebookFeedPaging Paging { get; set; }
    }


}
