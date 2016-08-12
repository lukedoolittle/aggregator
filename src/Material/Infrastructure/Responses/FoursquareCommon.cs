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
    public class FoursquareMeta
    {
        [DataMember(Name = "code")]
        [JsonProperty(PropertyName = "code")]
        public int Code { get; set; }
        [DataMember(Name = "requestId")]
        [JsonProperty(PropertyName = "requestId")]
        public string RequestId { get; set; }
    }

    [DataContract]
    public class FoursquareStats
    {
        [DataMember(Name = "checkinsCount")]
        [JsonProperty(PropertyName = "checkinsCount")]
        public int CheckinsCount { get; set; }
        [DataMember(Name = "usersCount")]
        [JsonProperty(PropertyName = "usersCount")]
        public int UsersCount { get; set; }
        [DataMember(Name = "tipCount")]
        [JsonProperty(PropertyName = "tipCount")]
        public int TipCount { get; set; }
    }
    [DataContract]
    public class FoursquareNotificationItem
    {
        [DataMember(Name = "unreadCount")]
        [JsonProperty(PropertyName = "unreadCount")]
        public int UnreadCount { get; set; }
    }

    [DataContract]
    public class FoursquareNotification
    {
        [DataMember(Name = "type")]
        [JsonProperty(PropertyName = "type")]
        public string Type { get; set; }
        [DataMember(Name = "item")]
        [JsonProperty(PropertyName = "item")]
        public FoursquareNotificationItem Item { get; set; }
    }

    [DataContract]
    public class FoursquareIcon
    {
        [DataMember(Name = "prefix")]
        [JsonProperty(PropertyName = "prefix")]
        public string Prefix { get; set; }
        [DataMember(Name = "suffix")]
        [JsonProperty(PropertyName = "suffix")]
        public string Suffix { get; set; }
    }

    [DataContract]
    public class FoursquareSaves
    {
        [DataMember(Name = "count")]
        [JsonProperty(PropertyName = "count")]
        public int Count { get; set; }
        [DataMember(Name = "groups")]
        [JsonProperty(PropertyName = "groups")]
        public IList<object> Groups { get; set; }
    }

    [DataContract]
    public class FoursquarePhotos
    {
        [DataMember(Name = "count")]
        [JsonProperty(PropertyName = "count")]
        public int Count { get; set; }
        [DataMember(Name = "groups")]
        [JsonProperty(PropertyName = "groups")]
        public IList<FoursquareGroup> Groups { get; set; }
        [DataMember(Name = "items")]
        [JsonProperty(PropertyName = "items")]
        public IList<object> Items { get; set; }
    }

    [DataContract]
    public class FoursquareGroup
    {
        [DataMember(Name = "type")]
        [JsonProperty(PropertyName = "type")]
        public string Type { get; set; }
        [DataMember(Name = "name")]
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }
        [DataMember(Name = "count")]
        [JsonProperty(PropertyName = "count")]
        public int Count { get; set; }
        [DataMember(Name = "items")]
        [JsonProperty(PropertyName = "items")]
        public IList<object> Items { get; set; }
    }

    [DataContract]
    public class FoursquareVenuePage
    {
        [DataMember(Name = "id")]
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }
    }

    [DataContract]
    public class FoursquarePrice
    {
        [DataMember(Name = "tier")]
        [JsonProperty(PropertyName = "tier")]
        public int Tier { get; set; }
        [DataMember(Name = "message")]
        [JsonProperty(PropertyName = "message")]
        public string Message { get; set; }
        [DataMember(Name = "currency")]
        [JsonProperty(PropertyName = "currency")]
        public string Currency { get; set; }
    }

    [DataContract]
    public class FoursquareVenue
    {
        [DataMember(Name = "id")]
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }
        [DataMember(Name = "name")]
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }
        [DataMember(Name = "location")]
        [JsonProperty(PropertyName = "location")]
        public FoursquareLocation Location { get; set; }
        [DataMember(Name = "categories")]
        [JsonProperty(PropertyName = "categories")]
        public IList<FoursquareCategory> Categories { get; set; }
        [DataMember(Name = "verified")]
        [JsonProperty(PropertyName = "verified")]
        public bool Verified { get; set; }
        [DataMember(Name = "stats")]
        [JsonProperty(PropertyName = "stats")]
        public FoursquareStats Stats { get; set; }
        [DataMember(Name = "price")]
        [JsonProperty(PropertyName = "price")]
        public FoursquarePrice Price { get; set; }
        [DataMember(Name = "hasMenu")]
        [JsonProperty(PropertyName = "hasMenu")]
        public bool HasMenu { get; set; }
        [DataMember(Name = "rating")]
        [JsonProperty(PropertyName = "rating")]
        public double Rating { get; set; }
        [DataMember(Name = "ratingSignals")]
        [JsonProperty(PropertyName = "ratingSignals")]
        public int RatingSignals { get; set; }
        [DataMember(Name = "allowMenuUrlEdit")]
        [JsonProperty(PropertyName = "allowMenuUrlEdit")]
        public bool AllowMenuUrlEdit { get; set; }
        [DataMember(Name = "photos")]
        [JsonProperty(PropertyName = "photos")]
        public FoursquarePhotos Photos { get; set; }
        [DataMember(Name = "venuePage")]
        [JsonProperty(PropertyName = "venuePage")]
        public FoursquareVenuePage VenuePage { get; set; }
        [DataMember(Name = "storeId")]
        [JsonProperty(PropertyName = "storeId")]
        public string StoreId { get; set; }
        public bool? like { get; set; }
        public bool? dislike { get; set; }
        public bool? ok { get; set; }
    }

    [DataContract]
    public class FoursquareLikes
    {
        [DataMember(Name = "count")]
        [JsonProperty(PropertyName = "count")]
        public int Count { get; set; }
        [DataMember(Name = "groups")]
        [JsonProperty(PropertyName = "groups")]
        public IList<object> Groups { get; set; }
        [DataMember(Name = "summary")]
        [JsonProperty(PropertyName = "summary")]
        public string Summary { get; set; }
    }
}
