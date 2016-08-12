using System.Runtime.Serialization;
using System.Collections.Generic;

namespace Material.Infrastructure.Responses
{
    [DataContract]
    public class TwentyThreeAndMeProfile
    {
        [DataMember(Name = "genotyped")]
        public bool Genotyped { get; set; }
        [DataMember(Name = "id")]
        public string Id { get; set; }
    }

    [DataContract]
    public class TwentyThreeAndMeUserResponse
    {
        [DataMember(Name = "id")]
        public string Id { get; set; }
        [DataMember(Name = "profiles")]
        public IList<TwentyThreeAndMeProfile> Profiles { get; set; }
    }
}
