using System.Runtime.Serialization;

namespace Material.Infrastructure.Responses
{
    [DataContract]
    public class TwentyThreeAndMeGenomeResponse
    {
        [DataMember(Name = "id")]
        public string Id { get; set; }
        [DataMember(Name = "genome")]
        public string Genome { get; set; }
    }
}
