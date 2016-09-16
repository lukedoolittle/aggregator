using System.Runtime.Serialization;

namespace Material.Infrastructure.Responses
{
    [DataContract]
    public class FacebookUserResponse
    {
        [DataMember(Name = "email")]
        public string Email { get; set; }

        [DataMember(Name = "id")]
        public string Id { get; set; }
    }
}
