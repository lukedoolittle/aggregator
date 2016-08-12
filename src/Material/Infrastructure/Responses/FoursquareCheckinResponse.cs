using System.Runtime.Serialization;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Material.Infrastructure.Responses
{
    [DataContract]
    public class FoursquareLocation
    {
        [DataMember(Name = "address")]
        [JsonProperty(PropertyName = "address")]
        public string Address { get; set; }
        [DataMember(Name = "lat")]
        [JsonProperty(PropertyName = "lat")]
        public double Lat { get; set; }
        [DataMember(Name = "lng")]
        [JsonProperty(PropertyName = "lng")]
        public double Lng { get; set; }
        [DataMember(Name = "postalCode")]
        [JsonProperty(PropertyName = "postalCode")]
        public string PostalCode { get; set; }
        [DataMember(Name = "cc")]
        [JsonProperty(PropertyName = "cc")]
        public string Cc { get; set; }
        [DataMember(Name = "city")]
        [JsonProperty(PropertyName = "city")]
        public string City { get; set; }
        [DataMember(Name = "state")]
        [JsonProperty(PropertyName = "state")]
        public string State { get; set; }
        [DataMember(Name = "country")]
        [JsonProperty(PropertyName = "country")]
        public string Country { get; set; }
        [DataMember(Name = "formattedAddress")]
        [JsonProperty(PropertyName = "formattedAddress")]
        public IList<string> FormattedAddress { get; set; }
        public bool? isFuzzed { get; set; }
    }

    [DataContract]
    public class FoursquareCategory
    {
        [DataMember(Name = "id")]
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }
        [DataMember(Name = "name")]
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }
        [DataMember(Name = "pluralName")]
        [JsonProperty(PropertyName = "pluralName")]
        public string PluralName { get; set; }
        [DataMember(Name = "shortName")]
        [JsonProperty(PropertyName = "shortName")]
        public string ShortName { get; set; }
        [DataMember(Name = "icon")]
        [JsonProperty(PropertyName = "icon")]
        public FoursquareIcon Icon { get; set; }
        [DataMember(Name = "primary")]
        [JsonProperty(PropertyName = "primary")]
        public bool Primary { get; set; }
    }


    [DataContract]
    public class FoursquarePosts
    {
        [DataMember(Name = "count")]
        [JsonProperty(PropertyName = "count")]
        public int Count { get; set; }
        [DataMember(Name = "textCount")]
        [JsonProperty(PropertyName = "textCount")]
        public int TextCount { get; set; }
    }

    [DataContract]
    public class FoursquareComments
    {
        [DataMember(Name = "count")]
        [JsonProperty(PropertyName = "count")]
        public int Count { get; set; }
    }

    [DataContract]
    public class FoursquareSource
    {
        [DataMember(Name = "name")]
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }
        [DataMember(Name = "url")]
        [JsonProperty(PropertyName = "url")]
        public string Url { get; set; }
    }

    [DataContract]
    public class FoursquareCheckinItem
    {
        [DataMember(Name = "id")]
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }
        [DataMember(Name = "createdAt")]
        [JsonProperty(PropertyName = "createdAt")]
        public int CreatedAt { get; set; }
        [DataMember(Name = "type")]
        [JsonProperty(PropertyName = "type")]
        public string Type { get; set; }
        [DataMember(Name = "timeZoneOffset")]
        [JsonProperty(PropertyName = "timeZoneOffset")]
        public int TimeZoneOffset { get; set; }
        [DataMember(Name = "venue")]
        [JsonProperty(PropertyName = "venue")]
        public FoursquareVenue Venue { get; set; }
        [DataMember(Name = "likes")]
        [JsonProperty(PropertyName = "likes")]
        public FoursquareLikes Likes { get; set; }
        [DataMember(Name = "like")]
        [JsonProperty(PropertyName = "like")]
        public bool Like { get; set; }
        [DataMember(Name = "isMayor")]
        [JsonProperty(PropertyName = "isMayor")]
        public bool IsMayor { get; set; }
        [DataMember(Name = "photos")]
        [JsonProperty(PropertyName = "photos")]
        public FoursquarePhotos Photos { get; set; }
        [DataMember(Name = "posts")]
        [JsonProperty(PropertyName = "posts")]
        public FoursquarePosts Posts { get; set; }
        [DataMember(Name = "comments")]
        [JsonProperty(PropertyName = "comments")]
        public FoursquareComments Comments { get; set; }
        [DataMember(Name = "source")]
        [JsonProperty(PropertyName = "source")]
        public FoursquareSource Source { get; set; }
    }

    [DataContract]
    public class FoursquareCheckins
    {
        [DataMember(Name = "count")]
        [JsonProperty(PropertyName = "count")]
        public int Count { get; set; }
        [DataMember(Name = "items")]
        [JsonProperty(PropertyName = "items")]
        public IList<FoursquareCheckinItem> Items { get; set; }
    }

    [DataContract]
    public class FoursquareResponse
    {
        [DataMember(Name = "checkins")]
        [JsonProperty(PropertyName = "checkins")]
        public FoursquareCheckins Checkins { get; set; }
    }

    [DataContract]
    public class FoursquareCheckinResponse
    {
        [DataMember(Name = "meta")]
        [JsonProperty(PropertyName = "meta")]
        public FoursquareMeta Meta { get; set; }
        [DataMember(Name = "notifications")]
        [JsonProperty(PropertyName = "notifications")]
        public IList<FoursquareNotification> Notifications { get; set; }
        [DataMember(Name = "response")]
        [JsonProperty(PropertyName = "response")]
        public FoursquareResponse Response { get; set; }
    }


}
