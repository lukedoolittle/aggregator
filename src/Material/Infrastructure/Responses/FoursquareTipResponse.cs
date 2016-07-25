using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Material.Infrastructure.Responses
{
    public class FoursquareTipItem
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }
        [JsonProperty(PropertyName = "createdAt")]
        public int CreatedAt { get; set; }
        [JsonProperty(PropertyName = "text")]
        public string Text { get; set; }
        [JsonProperty(PropertyName = "type")]
        public string Type { get; set; }
        [JsonProperty(PropertyName = "canonicalUrl")]
        public string CanonicalUrl { get; set; }
        [JsonProperty(PropertyName = "likes")]
        public FoursquareLikes Likes { get; set; }
        [JsonProperty(PropertyName = "like")]
        public bool Like { get; set; }
        [JsonProperty(PropertyName = "ratedAt")]
        public int RatedAt { get; set; }
        [JsonProperty(PropertyName = "viewCount")]
        public int ViewCount { get; set; }
        [JsonProperty(PropertyName = "saves")]
        public FoursquareSaves Saves { get; set; }
        [JsonProperty(PropertyName = "agreeCount")]
        public int AgreeCount { get; set; }
        [JsonProperty(PropertyName = "disagreeCount")]
        public int DisagreeCount { get; set; }
        [JsonProperty(PropertyName = "agreed")]
        public bool Agreed { get; set; }
        [JsonProperty(PropertyName = "venue")]
        public FoursquareVenue Venue { get; set; }
    }

    public class FoursquareTips
    {
        [JsonProperty(PropertyName = "count")]
        public int Count { get; set; }
        [JsonProperty(PropertyName = "items")]
        public IList<FoursquareTipItem> Items { get; set; }
    }

    public class FoursquareTipIntermediateResponse
    {
        [JsonProperty(PropertyName = "tips")]
        public FoursquareTips Tips { get; set; }
    }

    public class FoursquareTipResponse
    {
        [JsonProperty(PropertyName = "meta")]
        public FoursquareMeta Meta { get; set; }
        [JsonProperty(PropertyName = "notifications")]
        public IList<FoursquareNotification> Notifications { get; set; }
        [JsonProperty(PropertyName = "response")]
        public FoursquareTipIntermediateResponse Response { get; set; }
    }
}
