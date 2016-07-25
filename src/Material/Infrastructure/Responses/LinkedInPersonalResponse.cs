using Newtonsoft.Json;
namespace Material.Infrastructure.Responses
{
    public class LinkedInSiteStandardProfileRequest
    {
        [JsonProperty(PropertyName = "url")]
        public string Url { get; set; }
    }

    public class LinkedInPersonalResponse
    {
        [JsonProperty(PropertyName = "firstName")]
        public string FirstName { get; set; }
        [JsonProperty(PropertyName = "headline")]
        public string Headline { get; set; }
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }
        [JsonProperty(PropertyName = "lastName")]
        public string LastName { get; set; }
        [JsonProperty(PropertyName = "siteStandardProfileRequest")]
        public LinkedInSiteStandardProfileRequest SiteStandardProfileRequest { get; set; }
    }
}
