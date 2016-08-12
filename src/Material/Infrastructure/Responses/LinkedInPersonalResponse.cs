using System.Runtime.Serialization;
using Newtonsoft.Json;
namespace Material.Infrastructure.Responses
{
    [DataContract]
    public class LinkedInSiteStandardProfileRequest
    {
        [DataMember(Name = "url")]
        [JsonProperty(PropertyName = "url")]
        public string Url { get; set; }
    }

    [DataContract]
    public class LinkedInPersonalResponse
    {
        [DataMember(Name = "firstName")]
        [JsonProperty(PropertyName = "firstName")]
        public string FirstName { get; set; }
        [DataMember(Name = "headline")]
        [JsonProperty(PropertyName = "headline")]
        public string Headline { get; set; }
        [DataMember(Name = "id")]
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }
        [DataMember(Name = "lastName")]
        [JsonProperty(PropertyName = "lastName")]
        public string LastName { get; set; }
        [DataMember(Name = "siteStandardProfileRequest")]
        [JsonProperty(PropertyName = "siteStandardProfileRequest")]
        public LinkedInSiteStandardProfileRequest SiteStandardProfileRequest { get; set; }
    }
}
