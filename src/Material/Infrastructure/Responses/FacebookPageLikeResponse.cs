using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Material.Infrastructure.Responses
{
    public class FacebookLikeDatum
    {
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }
        [JsonProperty(PropertyName = "created_time")]
        public DateTime CreatedTime { get; set; }
    }

    public class FacebookCursors
    {
        [JsonProperty(PropertyName = "before")]
        public string Before { get; set; }
        [JsonProperty(PropertyName = "after")]
        public string After { get; set; }
    }

    public class FacebookPaging
    {
        [JsonProperty(PropertyName = "cursors")]
        public FacebookCursors Cursors { get; set; }
        [JsonProperty(PropertyName = "next")]
        public string Next { get; set; }
    }

    public class FacebookPageLikeResponse
    {
        [JsonProperty(PropertyName = "data")]
        public IList<FacebookLikeDatum> Data { get; set; }
        [JsonProperty(PropertyName = "paging")]
        public FacebookPaging Paging { get; set; }
    }


}
