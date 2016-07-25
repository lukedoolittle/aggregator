using Newtonsoft.Json;
using System.Collections.Generic;

namespace Material.Infrastructure.Responses
{
    public class FoursquareLocation
    {
        [JsonProperty(PropertyName = "address")]
        public string Address { get; set; }
        [JsonProperty(PropertyName = "lat")]
        public double Lat { get; set; }
        [JsonProperty(PropertyName = "lng")]
        public double Lng { get; set; }
        [JsonProperty(PropertyName = "postalCode")]
        public string PostalCode { get; set; }
        [JsonProperty(PropertyName = "cc")]
        public string Cc { get; set; }
        [JsonProperty(PropertyName = "city")]
        public string City { get; set; }
        [JsonProperty(PropertyName = "state")]
        public string State { get; set; }
        [JsonProperty(PropertyName = "country")]
        public string Country { get; set; }
        [JsonProperty(PropertyName = "formattedAddress")]
        public IList<string> FormattedAddress { get; set; }
        public bool? isFuzzed { get; set; }
    }

    public class FoursquareCategory
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }
        [JsonProperty(PropertyName = "pluralName")]
        public string PluralName { get; set; }
        [JsonProperty(PropertyName = "shortName")]
        public string ShortName { get; set; }
        [JsonProperty(PropertyName = "icon")]
        public FoursquareIcon Icon { get; set; }
        [JsonProperty(PropertyName = "primary")]
        public bool Primary { get; set; }
    }


    public class FoursquarePosts
    {
        [JsonProperty(PropertyName = "count")]
        public int Count { get; set; }
        [JsonProperty(PropertyName = "textCount")]
        public int TextCount { get; set; }
    }

    public class FoursquareComments
    {
        [JsonProperty(PropertyName = "count")]
        public int Count { get; set; }
    }

    public class FoursquareSource
    {
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }
        [JsonProperty(PropertyName = "url")]
        public string Url { get; set; }
    }

    public class FoursquareCheckinItem
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }
        [JsonProperty(PropertyName = "createdAt")]
        public int CreatedAt { get; set; }
        [JsonProperty(PropertyName = "type")]
        public string Type { get; set; }
        [JsonProperty(PropertyName = "timeZoneOffset")]
        public int TimeZoneOffset { get; set; }
        [JsonProperty(PropertyName = "venue")]
        public FoursquareVenue Venue { get; set; }
        [JsonProperty(PropertyName = "likes")]
        public FoursquareLikes Likes { get; set; }
        [JsonProperty(PropertyName = "like")]
        public bool Like { get; set; }
        [JsonProperty(PropertyName = "isMayor")]
        public bool IsMayor { get; set; }
        [JsonProperty(PropertyName = "photos")]
        public FoursquarePhotos Photos { get; set; }
        [JsonProperty(PropertyName = "posts")]
        public FoursquarePosts Posts { get; set; }
        [JsonProperty(PropertyName = "comments")]
        public FoursquareComments Comments { get; set; }
        [JsonProperty(PropertyName = "source")]
        public FoursquareSource Source { get; set; }
    }

    public class FoursquareCheckins
    {
        [JsonProperty(PropertyName = "count")]
        public int Count { get; set; }
        [JsonProperty(PropertyName = "items")]
        public IList<FoursquareCheckinItem> Items { get; set; }
    }

    public class FoursquareResponse
    {
        [JsonProperty(PropertyName = "checkins")]
        public FoursquareCheckins Checkins { get; set; }
    }

    public class FoursquareCheckinResponse
    {
        [JsonProperty(PropertyName = "meta")]
        public FoursquareMeta Meta { get; set; }
        [JsonProperty(PropertyName = "notifications")]
        public IList<FoursquareNotification> Notifications { get; set; }
        [JsonProperty(PropertyName = "response")]
        public FoursquareResponse Response { get; set; }
    }


}
