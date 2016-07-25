using Newtonsoft.Json;
using System.Collections.Generic;

namespace Material.Infrastructure.Responses
{
    public class FoursquarePhoto
    {
        [JsonProperty(PropertyName = "prefix")]
        public string Prefix { get; set; }
        [JsonProperty(PropertyName = "suffix")]
        public string Suffix { get; set; }
        public bool @default { get; set; }
    }

    public class FoursquareLists
    {
        [JsonProperty(PropertyName = "groups")]
        public IList<FoursquareGroup> Groups { get; set; }
    }

    public class FoursquareContact
    {
        [JsonProperty(PropertyName = "email")]
        public string Email { get; set; }
        [JsonProperty(PropertyName = "facebook")]
        public string Facebook { get; set; }
        [JsonProperty(PropertyName = "twitter")]
        public string Twitter { get; set; }
    }

    public class FoursquareFriendItem
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }
        [JsonProperty(PropertyName = "firstName")]
        public string FirstName { get; set; }
        [JsonProperty(PropertyName = "lastName")]
        public string LastName { get; set; }
        [JsonProperty(PropertyName = "gender")]
        public string Gender { get; set; }
        [JsonProperty(PropertyName = "relationship")]
        public string Relationship { get; set; }
        [JsonProperty(PropertyName = "photo")]
        public FoursquarePhoto Photo { get; set; }
        [JsonProperty(PropertyName = "tips")]
        public FoursquareTips Tips { get; set; }
        [JsonProperty(PropertyName = "lists")]
        public FoursquareLists Lists { get; set; }
        [JsonProperty(PropertyName = "homeCity")]
        public string HomeCity { get; set; }
        [JsonProperty(PropertyName = "bio")]
        public string Bio { get; set; }
        [JsonProperty(PropertyName = "contact")]
        public FoursquareContact Contact { get; set; }
    }

    public class FoursquareFriends
    {
        [JsonProperty(PropertyName = "count")]
        public int Count { get; set; }
        [JsonProperty(PropertyName = "items")]
        public IList<FoursquareFriendItem> Items { get; set; }
    }

    public class FoursquareFriendIntermediateResponse
    {
        [JsonProperty(PropertyName = "friends")]
        public FoursquareFriends Friends { get; set; }
        [JsonProperty(PropertyName = "checksum")]
        public string Checksum { get; set; }
    }

    public class FoursquareFriendResponse
    {
        [JsonProperty(PropertyName = "meta")]
        public FoursquareMeta Meta { get; set; }
        [JsonProperty(PropertyName = "notifications")]
        public IList<FoursquareNotification> Notifications { get; set; }
        [JsonProperty(PropertyName = "response")]
        public FoursquareFriendIntermediateResponse Response { get; set; }
    }
}
