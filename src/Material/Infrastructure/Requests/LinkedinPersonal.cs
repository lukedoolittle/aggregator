using Material.Metadata;
using Material.Infrastructure.ProtectedResources;
using System;
using System.Collections.Generic;
using Material.Infrastructure.Requests;
using Material.Enums;
using Material.Infrastructure;
using Foundations.Attributes;

namespace Material.Infrastructure.Requests
{     
    /// <summary>
    /// Basic profile data
    /// </summary>
    [ServiceType(typeof(LinkedIn))]
	public partial class LinkedinPersonal : OAuthRequest              
	{
        public override String Host => "https://api.linkedin.com";
        public override String Path => "/v1/people/~";
        public override String HttpMethod => "GET";
        public override List<String> RequiredScopes => new List<String> { "r_basicprofile" };
        /// <summary>
        /// the format of the response
        /// </summary>
        [Name("format")]
        [ParameterType(RequestParameterTypeEnum.Query)]
        public  LinkedinPersonalFormatEnum Format { get; set; } = LinkedinPersonalFormatEnum.Json;
	}
    public enum LinkedinPersonalFormatEnum
    {
        [Description("json")] Json,
        [Description("xml")] Xml,
    }
}
