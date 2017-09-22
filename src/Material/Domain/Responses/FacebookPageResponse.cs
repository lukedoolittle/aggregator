using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Material.Domain.Responses
{
    [GeneratedCode("T4Toolbox", "14.0")]
    [DataContract]
    public class FacebookEngagement
    {
        [DataMember(Name = "count")]
        public int Count { get; set; }

        [DataMember(Name = "social_sentence")]
        public string SocialSentence { get; set; }
    }

    [GeneratedCode("T4Toolbox", "14.0")]
    [DataContract]
    public class FacebookPageResponse
    {
        [DataMember(Name = "engagement")]
        public FacebookEngagement Engagement { get; set; }

        [DataMember(Name = "fan_count")]
        public int FanCount { get; set; }

        [DataMember(Name = "about")]
        public string About { get; set; }

        [DataMember(Name = "id")]
        public string Id { get; set; }
    }
}
