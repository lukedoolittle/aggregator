using Newtonsoft.Json;

namespace Material.Infrastructure.Responses
{
    public class TwentyThreeAndMeGenomeResponse
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }
        [JsonProperty(PropertyName = "genome")]
        public string Genome { get; set; }
    }
}
