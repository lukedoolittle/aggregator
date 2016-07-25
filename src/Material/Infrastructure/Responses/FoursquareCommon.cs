using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Material.Infrastructure.Responses
{
    public class FoursquareMeta
    {
        [JsonProperty(PropertyName = "code")]
        public int Code { get; set; }
        [JsonProperty(PropertyName = "requestId")]
        public string RequestId { get; set; }
    }

    public class FoursquareStats
    {
        [JsonProperty(PropertyName = "checkinsCount")]
        public int CheckinsCount { get; set; }
        [JsonProperty(PropertyName = "usersCount")]
        public int UsersCount { get; set; }
        [JsonProperty(PropertyName = "tipCount")]
        public int TipCount { get; set; }
    }
    public class FoursquareNotificationItem
    {
        [JsonProperty(PropertyName = "unreadCount")]
        public int UnreadCount { get; set; }
    }

    public class FoursquareNotification
    {
        [JsonProperty(PropertyName = "type")]
        public string Type { get; set; }
        [JsonProperty(PropertyName = "item")]
        public FoursquareNotificationItem Item { get; set; }
    }

    public class FoursquareIcon
    {
        [JsonProperty(PropertyName = "prefix")]
        public string Prefix { get; set; }
        [JsonProperty(PropertyName = "suffix")]
        public string Suffix { get; set; }
    }

    public class FoursquareSaves
    {
        [JsonProperty(PropertyName = "count")]
        public int Count { get; set; }
        [JsonProperty(PropertyName = "groups")]
        public IList<object> Groups { get; set; }
    }

    public class FoursquarePhotos
    {
        [JsonProperty(PropertyName = "count")]
        public int Count { get; set; }
        [JsonProperty(PropertyName = "groups")]
        public IList<FoursquareGroup> Groups { get; set; }
        [JsonProperty(PropertyName = "items")]
        public IList<object> Items { get; set; }
    }

    public class FoursquareGroup
    {
        [JsonProperty(PropertyName = "type")]
        public string Type { get; set; }
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }
        [JsonProperty(PropertyName = "count")]
        public int Count { get; set; }
        [JsonProperty(PropertyName = "items")]
        public IList<object> Items { get; set; }
    }

    public class FoursquareVenuePage
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }
    }

    public class FoursquarePrice
    {
        [JsonProperty(PropertyName = "tier")]
        public int Tier { get; set; }
        [JsonProperty(PropertyName = "message")]
        public string Message { get; set; }
        [JsonProperty(PropertyName = "currency")]
        public string Currency { get; set; }
    }

    public class FoursquareVenue
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }
        [JsonProperty(PropertyName = "location")]
        public FoursquareLocation Location { get; set; }
        [JsonProperty(PropertyName = "categories")]
        public IList<FoursquareCategory> Categories { get; set; }
        [JsonProperty(PropertyName = "verified")]
        public bool Verified { get; set; }
        [JsonProperty(PropertyName = "stats")]
        public FoursquareStats Stats { get; set; }
        [JsonProperty(PropertyName = "price")]
        public FoursquarePrice Price { get; set; }
        [JsonProperty(PropertyName = "hasMenu")]
        public bool HasMenu { get; set; }
        [JsonProperty(PropertyName = "rating")]
        public double Rating { get; set; }
        [JsonProperty(PropertyName = "ratingSignals")]
        public int RatingSignals { get; set; }
        [JsonProperty(PropertyName = "allowMenuUrlEdit")]
        public bool AllowMenuUrlEdit { get; set; }
        [JsonProperty(PropertyName = "photos")]
        public FoursquarePhotos Photos { get; set; }
        [JsonProperty(PropertyName = "venuePage")]
        public FoursquareVenuePage VenuePage { get; set; }
        [JsonProperty(PropertyName = "storeId")]
        public string StoreId { get; set; }
        public bool? like { get; set; }
        public bool? dislike { get; set; }
        public bool? ok { get; set; }
    }

    public class FoursquareLikes
    {
        [JsonProperty(PropertyName = "count")]
        public int Count { get; set; }
        [JsonProperty(PropertyName = "groups")]
        public IList<object> Groups { get; set; }
        [JsonProperty(PropertyName = "summary")]
        public string Summary { get; set; }
    }
}
