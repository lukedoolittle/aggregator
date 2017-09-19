using System.CodeDom.Compiler;
using System.Runtime.Serialization;

namespace Material.Domain.Responses
{
    [GeneratedCode("T4Toolbox", "14.0")]
    [DataContract]
    public class Counts
    {
        [DataMember(Name = "media")]
        public int Media { get; set; }

        [DataMember(Name = "follows")]
        public int Follows { get; set; }

        [DataMember(Name = "followed_by")]
        public int FollowedBy { get; set; }
    }

    [GeneratedCode("T4Toolbox", "14.0")]
    [DataContract]
    public class Data
    {
        [DataMember(Name = "id")]
        public string Id { get; set; }

        [DataMember(Name = "username")]
        public string Username { get; set; }

        [DataMember(Name = "full_name")]
        public string FullName { get; set; }

        [DataMember(Name = "profile_picture")]
        public string ProfilePicture { get; set; }

        [DataMember(Name = "bio")]
        public string Bio { get; set; }

        [DataMember(Name = "website")]
        public string Website { get; set; }

        [DataMember(Name = "counts")]
        public Counts Counts { get; set; }
    }

    [GeneratedCode("T4Toolbox", "14.0")]
    [DataContract]
    public class InstagramUserResponse
    {
        [DataMember(Name = "data")]
        public Data Data { get; set; }
    }
}
