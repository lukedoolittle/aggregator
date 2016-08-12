using System.Runtime.Serialization;
namespace Material.Infrastructure.Responses
{
    [DataContract]
    public class LinkedInSiteStandardProfileRequest
    {
        [DataMember(Name = "url")]
        public string Url { get; set; }
    }

    [DataContract]
    public class LinkedInPersonalResponse
    {
        [DataMember(Name = "firstName")]
        public string FirstName { get; set; }
        [DataMember(Name = "headline")]
        public string Headline { get; set; }
        [DataMember(Name = "id")]
        public string Id { get; set; }
        [DataMember(Name = "lastName")]
        public string LastName { get; set; }
        [DataMember(Name = "siteStandardProfileRequest")]
        public LinkedInSiteStandardProfileRequest SiteStandardProfileRequest { get; set; }
    }
}
