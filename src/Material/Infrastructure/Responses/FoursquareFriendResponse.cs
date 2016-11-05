using System.CodeDom.Compiler;
using System.Runtime.Serialization;
using System.Collections.Generic;

namespace Material.Infrastructure.Responses
{
    [GeneratedCode("T4Toolbox", "14.0")]
    [DataContract]
    public class FoursquarePhoto
    {
        [DataMember(Name = "prefix")]
        public string Prefix { get; set; }
        [DataMember(Name = "suffix")]
        public string Suffix { get; set; }
        public bool @default { get; set; }
    }

    [GeneratedCode("T4Toolbox", "14.0")]
    [DataContract]
    public class FoursquareLists
    {
        [DataMember(Name = "groups")]
        public IList<FoursquareGroup> Groups { get; set; }
    }

    [GeneratedCode("T4Toolbox", "14.0")]
    [DataContract]
    public class FoursquareContact
    {
        [DataMember(Name = "email")]
        public string Email { get; set; }
        [DataMember(Name = "facebook")]
        public string Facebook { get; set; }
        [DataMember(Name = "twitter")]
        public string Twitter { get; set; }
    }

    [GeneratedCode("T4Toolbox", "14.0")]
    [DataContract]
    public class FoursquareFriendItem
    {
        [DataMember(Name = "id")]
        public string Id { get; set; }
        [DataMember(Name = "firstName")]
        public string FirstName { get; set; }
        [DataMember(Name = "lastName")]
        public string LastName { get; set; }
        [DataMember(Name = "gender")]
        public string Gender { get; set; }
        [DataMember(Name = "relationship")]
        public string Relationship { get; set; }
        [DataMember(Name = "photo")]
        public FoursquarePhoto Photo { get; set; }
        [DataMember(Name = "tips")]
        public FoursquareTips Tips { get; set; }
        [DataMember(Name = "lists")]
        public FoursquareLists Lists { get; set; }
        [DataMember(Name = "homeCity")]
        public string HomeCity { get; set; }
        [DataMember(Name = "bio")]
        public string Bio { get; set; }
        [DataMember(Name = "contact")]
        public FoursquareContact Contact { get; set; }
    }

    [GeneratedCode("T4Toolbox", "14.0")]
    [DataContract]
    public class FoursquareFriends
    {
        [DataMember(Name = "count")]
        public int Count { get; set; }
        [DataMember(Name = "items")]
        public IList<FoursquareFriendItem> Items { get; set; }
    }

    [GeneratedCode("T4Toolbox", "14.0")]
    [DataContract]
    public class FoursquareFriendIntermediateResponse
    {
        [DataMember(Name = "friends")]
        public FoursquareFriends Friends { get; set; }
        [DataMember(Name = "checksum")]
        public string Checksum { get; set; }
    }

    [GeneratedCode("T4Toolbox", "14.0")]
    [DataContract]
    public class FoursquareFriendResponse
    {
        [DataMember(Name = "meta")]
        public FoursquareMeta Meta { get; set; }
        [DataMember(Name = "notifications")]
        public IList<FoursquareNotification> Notifications { get; set; }
        [DataMember(Name = "response")]
        public FoursquareFriendIntermediateResponse Response { get; set; }
    }
}
