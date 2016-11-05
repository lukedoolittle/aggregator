using System.CodeDom.Compiler;
using System.Runtime.Serialization;
using System.Collections.Generic;

namespace Material.Infrastructure.Responses
{
    [GeneratedCode("T4Toolbox", "14.0")]
    [DataContract]
    public class TwentyThreeAndMeProfile
    {
        [DataMember(Name = "genotyped")]
        public bool Genotyped { get; set; }
        [DataMember(Name = "id")]
        public string Id { get; set; }
    }

    [GeneratedCode("T4Toolbox", "14.0")]
    [DataContract]
    public class TwentyThreeAndMeUserResponse
    {
        [DataMember(Name = "id")]
        public string Id { get; set; }
        [DataMember(Name = "profiles")]
        public IList<TwentyThreeAndMeProfile> Profiles { get; set; }
    }
}
