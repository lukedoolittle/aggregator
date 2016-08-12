using System.Runtime.Serialization;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Material.Infrastructure.Responses
{
    [DataContract]
    public class FoursquarePhoto
    {
        [DataMember(Name = "prefix")]
        [JsonProperty(PropertyName = "prefix")]
        public string Prefix { get; set; }
        [DataMember(Name = "suffix")]
        [JsonProperty(PropertyName = "suffix")]
        public string Suffix { get; set; }
        public bool @default { get; set; }
    }

    [DataContract]
    public class FoursquareLists
    {
        [DataMember(Name = "groups")]
        [JsonProperty(PropertyName = "groups")]
        public IList<FoursquareGroup> Groups { get; set; }
    }

    [DataContract]
    public class FoursquareContact
    {
        [DataMember(Name = "email")]
        [JsonProperty(PropertyName = "email")]
        public string Email { get; set; }
        [DataMember(Name = "facebook")]
        [JsonProperty(PropertyName = "facebook")]
        public string Facebook { get; set; }
        [DataMember(Name = "twitter")]
        [JsonProperty(PropertyName = "twitter")]
        public string Twitter { get; set; }
    }

    [DataContract]
    public class FoursquareFriendItem
    {
        [DataMember(Name = "id")]
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }
        [DataMember(Name = "firstName")]
        [JsonProperty(PropertyName = "firstName")]
        public string FirstName { get; set; }
        [DataMember(Name = "lastName")]
        [JsonProperty(PropertyName = "lastName")]
        public string LastName { get; set; }
        [DataMember(Name = "gender")]
        [JsonProperty(PropertyName = "gender")]
        public string Gender { get; set; }
        [DataMember(Name = "relationship")]
        [JsonProperty(PropertyName = "relationship")]
        public string Relationship { get; set; }
        [DataMember(Name = "photo")]
        [JsonProperty(PropertyName = "photo")]
        public FoursquarePhoto Photo { get; set; }
        [DataMember(Name = "tips")]
        [JsonProperty(PropertyName = "tips")]
        public FoursquareTips Tips { get; set; }
        [DataMember(Name = "lists")]
        [JsonProperty(PropertyName = "lists")]
        public FoursquareLists Lists { get; set; }
        [DataMember(Name = "homeCity")]
        [JsonProperty(PropertyName = "homeCity")]
        public string HomeCity { get; set; }
        [DataMember(Name = "bio")]
        [JsonProperty(PropertyName = "bio")]
        public string Bio { get; set; }
        [DataMember(Name = "contact")]
        [JsonProperty(PropertyName = "contact")]
        public FoursquareContact Contact { get; set; }
    }

    [DataContract]
    public class FoursquareFriends
    {
        [DataMember(Name = "count")]
        [JsonProperty(PropertyName = "count")]
        public int Count { get; set; }
        [DataMember(Name = "items")]
        [JsonProperty(PropertyName = "items")]
        public IList<FoursquareFriendItem> Items { get; set; }
    }

    [DataContract]
    public class FoursquareFriendIntermediateResponse
    {
        [DataMember(Name = "friends")]
        [JsonProperty(PropertyName = "friends")]
        public FoursquareFriends Friends { get; set; }
        [DataMember(Name = "checksum")]
        [JsonProperty(PropertyName = "checksum")]
        public string Checksum { get; set; }
    }

    [DataContract]
    public class FoursquareFriendResponse
    {
        [DataMember(Name = "meta")]
        [JsonProperty(PropertyName = "meta")]
        public FoursquareMeta Meta { get; set; }
        [DataMember(Name = "notifications")]
        [JsonProperty(PropertyName = "notifications")]
        public IList<FoursquareNotification> Notifications { get; set; }
        [DataMember(Name = "response")]
        [JsonProperty(PropertyName = "response")]
        public FoursquareFriendIntermediateResponse Response { get; set; }
    }
}
