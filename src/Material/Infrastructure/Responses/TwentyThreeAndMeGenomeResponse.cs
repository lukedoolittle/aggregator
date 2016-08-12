using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Material.Infrastructure.Responses
{
    [DataContract]
    public class TwentyThreeAndMeGenomeResponse
    {
        [DataMember(Name = "id")]
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }
        [DataMember(Name = "genome")]
        [JsonProperty(PropertyName = "genome")]
        public string Genome { get; set; }
    }
}
