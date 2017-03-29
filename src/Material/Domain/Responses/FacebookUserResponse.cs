using System.CodeDom.Compiler;
using System.Runtime.Serialization;

namespace Material.Domain.Responses
{
    [GeneratedCode("T4Toolbox", "14.0")]
    [DataContract]
    public class FacebookUserResponse
    {
        [DataMember(Name = "email")]
        public string Email { get; set; }

        [DataMember(Name = "id")]
        public string Id { get; set; }
    }
}
