using System.Runtime.Serialization;

namespace Material.Infrastructure.Responses
{
    [DataContract]
    public class AmazonProfileResponse
    {

        [DataMember(Name = "user_id")]
        public string UserId { get; set; }

        [DataMember(Name = "name")]
        public string Name { get; set; }

        [DataMember(Name = "email")]
        public string Email { get; set; }
    }
}
