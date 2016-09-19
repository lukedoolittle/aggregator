using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Material.Infrastructure.Responses
{
    [DataContract]
    public class GoogleEmail
    {

        [DataMember(Name = "value")]
        public string Value { get; set; }

        [DataMember(Name = "type")]
        public string Type { get; set; }
    }

    [DataContract]
    public class GoogleUrl
    {

        [DataMember(Name = "value")]
        public string Value { get; set; }

        [DataMember(Name = "type")]
        public string Type { get; set; }

        [DataMember(Name = "label")]
        public string Label { get; set; }
    }

    [DataContract]
    public class GoogleName
    {

        [DataMember(Name = "familyName")]
        public string FamilyName { get; set; }

        [DataMember(Name = "givenName")]
        public string GivenName { get; set; }
    }

    [DataContract]
    public class GoogleImage
    {

        [DataMember(Name = "url")]
        public string Url { get; set; }

        [DataMember(Name = "isDefault")]
        public bool IsDefault { get; set; }
    }

    [DataContract]
    public class GoogleProfileResponse
    {

        [DataMember(Name = "kind")]
        public string Kind { get; set; }

        [DataMember(Name = "etag")]
        public string Etag { get; set; }

        [DataMember(Name = "gender")]
        public string Gender { get; set; }

        [DataMember(Name = "emails")]
        public IList<GoogleEmail> Emails { get; set; }

        [DataMember(Name = "urls")]
        public IList<GoogleUrl> Urls { get; set; }

        [DataMember(Name = "objectType")]
        public string ObjectType { get; set; }

        [DataMember(Name = "id")]
        public string Id { get; set; }

        [DataMember(Name = "displayName")]
        public string DisplayName { get; set; }

        [DataMember(Name = "name")]
        public GoogleName Name { get; set; }

        [DataMember(Name = "tagline")]
        public string Tagline { get; set; }

        [DataMember(Name = "braggingRights")]
        public string BraggingRights { get; set; }

        [DataMember(Name = "url")]
        public string Url { get; set; }

        [DataMember(Name = "image")]
        public GoogleImage Image { get; set; }

        [DataMember(Name = "isPlusUser")]
        public bool IsPlusUser { get; set; }

        [DataMember(Name = "circledByCount")]
        public int CircledByCount { get; set; }

        [DataMember(Name = "verified")]
        public bool Verified { get; set; }
    }


}
