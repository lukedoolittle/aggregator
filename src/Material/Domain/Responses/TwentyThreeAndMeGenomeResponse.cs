using System.CodeDom.Compiler;
using System.Runtime.Serialization;

namespace Material.Domain.Responses
{
    [GeneratedCode("T4Toolbox", "14.0")]
    [DataContract]
    public class TwentyThreeAndMeGenomeResponse
    {
        [DataMember(Name = "id")]
        public string Id { get; set; }
        [DataMember(Name = "genome")]
        public string Genome { get; set; }
    }
}
