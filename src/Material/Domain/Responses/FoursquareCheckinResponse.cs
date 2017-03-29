using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Material.Domain.Responses
{
    [GeneratedCode("T4Toolbox", "14.0")]
    [DataContract]
    public class FoursquareLocation
    {
        [DataMember(Name = "address")]
        public string Address { get; set; }
        [DataMember(Name = "lat")]
        public double Lat { get; set; }
        [DataMember(Name = "lng")]
        public double Lng { get; set; }
        [DataMember(Name = "postalCode")]
        public string PostalCode { get; set; }
        [DataMember(Name = "cc")]
        public string Cc { get; set; }
        [DataMember(Name = "city")]
        public string City { get; set; }
        [DataMember(Name = "state")]
        public string State { get; set; }
        [DataMember(Name = "country")]
        public string Country { get; set; }
        [DataMember(Name = "formattedAddress")]
        public IList<string> FormattedAddress { get; set; }
        public bool? isFuzzed { get; set; }
    }

    [GeneratedCode("T4Toolbox", "14.0")]
    [DataContract]
    public class FoursquareCategory
    {
        [DataMember(Name = "id")]
        public string Id { get; set; }
        [DataMember(Name = "name")]
        public string Name { get; set; }
        [DataMember(Name = "pluralName")]
        public string PluralName { get; set; }
        [DataMember(Name = "shortName")]
        public string ShortName { get; set; }
        [DataMember(Name = "icon")]
        public FoursquareIcon Icon { get; set; }
        [DataMember(Name = "primary")]
        public bool Primary { get; set; }
    }

    [GeneratedCode("T4Toolbox", "14.0")]
    [DataContract]
    public class FoursquarePosts
    {
        [DataMember(Name = "count")]
        public int Count { get; set; }
        [DataMember(Name = "textCount")]
        public int TextCount { get; set; }
    }

    [GeneratedCode("T4Toolbox", "14.0")]
    [DataContract]
    public class FoursquareComments
    {
        [DataMember(Name = "count")]
        public int Count { get; set; }
    }

    [GeneratedCode("T4Toolbox", "14.0")]
    [DataContract]
    public class FoursquareSource
    {
        [DataMember(Name = "name")]
        public string Name { get; set; }
        [DataMember(Name = "url")]
        public string Url { get; set; }
    }

    [GeneratedCode("T4Toolbox", "14.0")]
    [DataContract]
    public class FoursquareCheckinItem
    {
        [DataMember(Name = "id")]
        public string Id { get; set; }
        [DataMember(Name = "createdAt")]
        public int CreatedAt { get; set; }
        [DataMember(Name = "type")]
        public string Type { get; set; }
        [DataMember(Name = "timeZoneOffset")]
        public int TimeZoneOffset { get; set; }
        [DataMember(Name = "venue")]
        public FoursquareVenue Venue { get; set; }
        [DataMember(Name = "likes")]
        public FoursquareLikes Likes { get; set; }
        [DataMember(Name = "like")]
        public bool Like { get; set; }
        [DataMember(Name = "isMayor")]
        public bool IsMayor { get; set; }
        [DataMember(Name = "photos")]
        public FoursquarePhotos Photos { get; set; }
        [DataMember(Name = "posts")]
        public FoursquarePosts Posts { get; set; }
        [DataMember(Name = "comments")]
        public FoursquareComments Comments { get; set; }
        [DataMember(Name = "source")]
        public FoursquareSource Source { get; set; }
    }

    [GeneratedCode("T4Toolbox", "14.0")]
    [DataContract]
    public class FoursquareCheckins
    {
        [DataMember(Name = "count")]
        public int Count { get; set; }
        [DataMember(Name = "items")]
        public IList<FoursquareCheckinItem> Items { get; set; }
    }

    [GeneratedCode("T4Toolbox", "14.0")]
    [DataContract]
    public class FoursquareResponse
    {
        [DataMember(Name = "checkins")]
        public FoursquareCheckins Checkins { get; set; }
    }

    [GeneratedCode("T4Toolbox", "14.0")]
    [DataContract]
    public class FoursquareCheckinResponse
    {
        [DataMember(Name = "meta")]
        public FoursquareMeta Meta { get; set; }
        [DataMember(Name = "notifications")]
        public IList<FoursquareNotification> Notifications { get; set; }
        [DataMember(Name = "response")]
        public FoursquareResponse Response { get; set; }
    }
}
