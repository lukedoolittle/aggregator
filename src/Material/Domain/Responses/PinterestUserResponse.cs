using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Material.Domain.Responses
{
    [GeneratedCode("T4Toolbox", "14.0")]
    [DataContract]
    public class PinterestImage
    {
        [DataMember(Name = "url")]
        public string Url { get; set; }

        [DataMember(Name = "width")]
        public int Width { get; set; }

        [DataMember(Name = "height")]
        public int Height { get; set; }
    }

    [GeneratedCode("T4Toolbox", "14.0")]
    [DataContract]
    public class PinterestUserCounts
    {
        [DataMember(Name = "pins")]
        public int Pins { get; set; }

        [DataMember(Name = "following")]
        public int Following { get; set; }

        [DataMember(Name = "followers")]
        public int Followers { get; set; }

        [DataMember(Name = "boards")]
        public int Boards { get; set; }
    }

    [GeneratedCode("T4Toolbox", "14.0")]
    [DataContract]
    public class PinterestUserDatum
    {
        [DataMember(Name = "username")]
        public string Username { get; set; }

        [DataMember(Name = "bio")]
        public string Bio { get; set; }

        [DataMember(Name = "first_name")]
        public string FirstName { get; set; }

        [DataMember(Name = "last_name")]
        public string LastName { get; set; }

        [DataMember(Name = "created_at")]
        public string CreatedAt { get; set; }

        [DataMember(Name = "image")]
        public IDictionary<string, PinterestImage> Image { get; set; }

        [DataMember(Name = "counts")]
        public PinterestUserCounts Counts { get; set; }

        [DataMember(Name = "id")]
        public string Id { get; set; }

        [DataMember(Name = "url")]
        public string Url { get; set; }
    }

    [GeneratedCode("T4Toolbox", "14.0")]
    [DataContract]
    public class PinterestUserResponse
    {
        [DataMember(Name = "data")]
        public PinterestUserDatum Data { get; set; }
    }
}
