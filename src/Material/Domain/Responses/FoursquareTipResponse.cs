using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Material.Domain.Responses
{
    [GeneratedCode("T4Toolbox", "14.0")]
    [DataContract]
    public class FoursquareTipItem
    {
        [DataMember(Name = "id")]
        public string Id { get; set; }
        [DataMember(Name = "createdAt")]
        public int CreatedAt { get; set; }
        [DataMember(Name = "text")]
        public string Text { get; set; }
        [DataMember(Name = "type")]
        public string Type { get; set; }
        [DataMember(Name = "canonicalUrl")]
        public string CanonicalUrl { get; set; }
        [DataMember(Name = "likes")]
        public FoursquareLikes Likes { get; set; }
        [DataMember(Name = "like")]
        public bool Like { get; set; }
        [DataMember(Name = "ratedAt")]
        public int RatedAt { get; set; }
        [DataMember(Name = "viewCount")]
        public int ViewCount { get; set; }
        [DataMember(Name = "saves")]
        public FoursquareSaves Saves { get; set; }
        [DataMember(Name = "agreeCount")]
        public int AgreeCount { get; set; }
        [DataMember(Name = "disagreeCount")]
        public int DisagreeCount { get; set; }
        [DataMember(Name = "agreed")]
        public bool Agreed { get; set; }
        [DataMember(Name = "venue")]
        public FoursquareVenue Venue { get; set; }
    }

    [GeneratedCode("T4Toolbox", "14.0")]
    [DataContract]
    public class FoursquareTips
    {
        [DataMember(Name = "count")]
        public int Count { get; set; }
        [DataMember(Name = "items")]
        public IList<FoursquareTipItem> Items { get; set; }
    }

    [GeneratedCode("T4Toolbox", "14.0")]
    [DataContract]
    public class FoursquareTipIntermediateResponse
    {
        [DataMember(Name = "tips")]
        public FoursquareTips Tips { get; set; }
    }

    [GeneratedCode("T4Toolbox", "14.0")]
    [DataContract]
    public class FoursquareTipResponse
    {
        [DataMember(Name = "meta")]
        public FoursquareMeta Meta { get; set; }
        [DataMember(Name = "notifications")]
        public IList<FoursquareNotification> Notifications { get; set; }
        [DataMember(Name = "response")]
        public FoursquareTipIntermediateResponse Response { get; set; }
    }
}
