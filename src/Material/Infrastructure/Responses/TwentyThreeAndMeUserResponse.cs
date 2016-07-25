using Newtonsoft.Json;
using System.Collections.Generic;

namespace Material.Infrastructure.Responses
{
    public class TwentyThreeAndMeProfile
    {
        [JsonProperty(PropertyName = "genotyped")]
        public bool Genotyped { get; set; }
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }
    }

    public class TwentyThreeAndMeUserResponse
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }
        [JsonProperty(PropertyName = "profiles")]
        public IList<TwentyThreeAndMeProfile> Profiles { get; set; }
    }
}
