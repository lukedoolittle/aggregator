using Material.Metadata;
using Material.Infrastructure.ProtectedResources;
using System;
using System.Collections.Generic;
using Material.Enums;
using Material.Infrastructure;

namespace Material.Infrastructure.Requests
{     
    [ServiceType(typeof(LinkedIn))]
	public partial class LinkedinUpdate : OAuthRequest              
	{
        public override String Host => "https://api.linkedin.com";
        public override String Path => "/v1/people/~/network/updates";
        public override String HttpMethod => "GET";
        public override List<String> RequiredScopes => new List<String>();
        [Name("format")]
        [ParameterType(RequestParameterTypeEnum.Query)]
        public  String Format { get; set; } = "json";
        [Name("modified")]
        [ParameterType(RequestParameterTypeEnum.Query)]
        public  String Modified { get; set; } = "new";
	}
}
