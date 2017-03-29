using System.CodeDom.Compiler;
using System.Runtime.Serialization;

namespace Material.Domain.Responses
{
    [GeneratedCode("T4Toolbox", "14.0")]
    [DataContract]
    public class LinkedInSiteStandardProfileRequest
    {
        [DataMember(Name = "url")]
        public string Url { get; set; }
    }

    [GeneratedCode("T4Toolbox", "14.0")]
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
