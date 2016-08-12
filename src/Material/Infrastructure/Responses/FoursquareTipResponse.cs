using System.Runtime.Serialization;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Material.Infrastructure.Responses
{
    [DataContract]
    public class FoursquareTipItem
    {
        [DataMember(Name = "id")]
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }
        [DataMember(Name = "createdAt")]
        [JsonProperty(PropertyName = "createdAt")]
        public int CreatedAt { get; set; }
        [DataMember(Name = "text")]
        [JsonProperty(PropertyName = "text")]
        public string Text { get; set; }
        [DataMember(Name = "type")]
        [JsonProperty(PropertyName = "type")]
        public string Type { get; set; }
        [DataMember(Name = "canonicalUrl")]
        [JsonProperty(PropertyName = "canonicalUrl")]
        public string CanonicalUrl { get; set; }
        [DataMember(Name = "likes")]
        [JsonProperty(PropertyName = "likes")]
        public FoursquareLikes Likes { get; set; }
        [DataMember(Name = "like")]
        [JsonProperty(PropertyName = "like")]
        public bool Like { get; set; }
        [DataMember(Name = "ratedAt")]
        [JsonProperty(PropertyName = "ratedAt")]
        public int RatedAt { get; set; }
        [DataMember(Name = "viewCount")]
        [JsonProperty(PropertyName = "viewCount")]
        public int ViewCount { get; set; }
        [DataMember(Name = "saves")]
        [JsonProperty(PropertyName = "saves")]
        public FoursquareSaves Saves { get; set; }
        [DataMember(Name = "agreeCount")]
        [JsonProperty(PropertyName = "agreeCount")]
        public int AgreeCount { get; set; }
        [DataMember(Name = "disagreeCount")]
        [JsonProperty(PropertyName = "disagreeCount")]
        public int DisagreeCount { get; set; }
        [DataMember(Name = "agreed")]
        [JsonProperty(PropertyName = "agreed")]
        public bool Agreed { get; set; }
        [DataMember(Name = "venue")]
        [JsonProperty(PropertyName = "venue")]
        public FoursquareVenue Venue { get; set; }
    }

    [DataContract]
    public class FoursquareTips
    {
        [DataMember(Name = "count")]
        [JsonProperty(PropertyName = "count")]
        public int Count { get; set; }
        [DataMember(Name = "items")]
        [JsonProperty(PropertyName = "items")]
        public IList<FoursquareTipItem> Items { get; set; }
    }

    [DataContract]
    public class FoursquareTipIntermediateResponse
    {
        [DataMember(Name = "tips")]
        [JsonProperty(PropertyName = "tips")]
        public FoursquareTips Tips { get; set; }
    }

    [DataContract]
    public class FoursquareTipResponse
    {
        [DataMember(Name = "meta")]
        [JsonProperty(PropertyName = "meta")]
        public FoursquareMeta Meta { get; set; }
        [DataMember(Name = "notifications")]
        [JsonProperty(PropertyName = "notifications")]
        public IList<FoursquareNotification> Notifications { get; set; }
        [DataMember(Name = "response")]
        [JsonProperty(PropertyName = "response")]
        public FoursquareTipIntermediateResponse Response { get; set; }
    }
}
