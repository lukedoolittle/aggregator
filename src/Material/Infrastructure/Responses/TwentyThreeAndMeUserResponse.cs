using System.Runtime.Serialization;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Material.Infrastructure.Responses
{
    [DataContract]
    public class TwentyThreeAndMeProfile
    {
        [DataMember(Name = "genotyped")]
        [JsonProperty(PropertyName = "genotyped")]
        public bool Genotyped { get; set; }
        [DataMember(Name = "id")]
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }
    }

    [DataContract]
    public class TwentyThreeAndMeUserResponse
    {
        [DataMember(Name = "id")]
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }
        [DataMember(Name = "profiles")]
        [JsonProperty(PropertyName = "profiles")]
        public IList<TwentyThreeAndMeProfile> Profiles { get; set; }
    }
}
