using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Material.Domain.Responses
{
    [GeneratedCode("T4Toolbox", "14.0")]
    [DataContract]
    public class FoursquareMeta
    {
        [DataMember(Name = "code")]
        public int Code { get; set; }
        [DataMember(Name = "requestId")]
        public string RequestId { get; set; }
    }

    [GeneratedCode("T4Toolbox", "14.0")]
    [DataContract]
    public class FoursquareStats
    {
        [DataMember(Name = "checkinsCount")]
        public int CheckinsCount { get; set; }
        [DataMember(Name = "usersCount")]
        public int UsersCount { get; set; }
        [DataMember(Name = "tipCount")]
        public int TipCount { get; set; }
    }

    [GeneratedCode("T4Toolbox", "14.0")]
    [DataContract]
    public class FoursquareNotificationItem
    {
        [DataMember(Name = "unreadCount")]
        public int UnreadCount { get; set; }
    }

    [GeneratedCode("T4Toolbox", "14.0")]
    [DataContract]
    public class FoursquareNotification
    {
        [DataMember(Name = "type")]
        public string Type { get; set; }
        [DataMember(Name = "item")]
        public FoursquareNotificationItem Item { get; set; }
    }

    [GeneratedCode("T4Toolbox", "14.0")]
    [DataContract]
    public class FoursquareIcon
    {
        [DataMember(Name = "prefix")]
        public string Prefix { get; set; }
        [DataMember(Name = "suffix")]
        public string Suffix { get; set; }
    }

    [GeneratedCode("T4Toolbox", "14.0")]
    [DataContract]
    public class FoursquareSaves
    {
        [DataMember(Name = "count")]
        public int Count { get; set; }
        [DataMember(Name = "groups")]
        public IList<object> Groups { get; set; }
    }

    [GeneratedCode("T4Toolbox", "14.0")]
    [DataContract]
    public class FoursquarePhotos
    {
        [DataMember(Name = "count")]
        public int Count { get; set; }
        [DataMember(Name = "groups")]
        public IList<FoursquareGroup> Groups { get; set; }
        [DataMember(Name = "items")]
        public IList<object> Items { get; set; }
    }

    [GeneratedCode("T4Toolbox", "14.0")]
    [DataContract]
    public class FoursquareGroup
    {
        [DataMember(Name = "type")]
        public string Type { get; set; }
        [DataMember(Name = "name")]
        public string Name { get; set; }
        [DataMember(Name = "count")]
        public int Count { get; set; }
        [DataMember(Name = "items")]
        public IList<object> Items { get; set; }
    }

    [GeneratedCode("T4Toolbox", "14.0")]
    [DataContract]
    public class FoursquareVenuePage
    {
        [DataMember(Name = "id")]
        public string Id { get; set; }
    }

    [GeneratedCode("T4Toolbox", "14.0")]
    [DataContract]
    public class FoursquarePrice
    {
        [DataMember(Name = "tier")]
        public int Tier { get; set; }
        [DataMember(Name = "message")]
        public string Message { get; set; }
        [DataMember(Name = "currency")]
        public string Currency { get; set; }
    }

    [GeneratedCode("T4Toolbox", "14.0")]
    [DataContract]
    public class FoursquareVenue
    {
        [DataMember(Name = "id")]
        public string Id { get; set; }
        [DataMember(Name = "name")]
        public string Name { get; set; }
        [DataMember(Name = "location")]
        public FoursquareLocation Location { get; set; }
        [DataMember(Name = "categories")]
        public IList<FoursquareCategory> Categories { get; set; }
        [DataMember(Name = "verified")]
        public bool Verified { get; set; }
        [DataMember(Name = "stats")]
        public FoursquareStats Stats { get; set; }
        [DataMember(Name = "price")]
        public FoursquarePrice Price { get; set; }
        [DataMember(Name = "hasMenu")]
        public bool HasMenu { get; set; }
        [DataMember(Name = "rating")]
        public double Rating { get; set; }
        [DataMember(Name = "ratingSignals")]
        public int RatingSignals { get; set; }
        [DataMember(Name = "allowMenuUrlEdit")]
        public bool AllowMenuUrlEdit { get; set; }
        [DataMember(Name = "photos")]
        public FoursquarePhotos Photos { get; set; }
        [DataMember(Name = "venuePage")]
        public FoursquareVenuePage VenuePage { get; set; }
        [DataMember(Name = "storeId")]
        public string StoreId { get; set; }
        public bool? like { get; set; }
        public bool? dislike { get; set; }
        public bool? ok { get; set; }
    }

    [GeneratedCode("T4Toolbox", "14.0")]
    [DataContract]
    public class FoursquareLikes
    {
        [DataMember(Name = "count")]
        public int Count { get; set; }
        [DataMember(Name = "groups")]
        public IList<object> Groups { get; set; }
        [DataMember(Name = "summary")]
        public string Summary { get; set; }
    }
}
