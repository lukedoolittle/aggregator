using Material.Metadata;
using System;
using Material.Infrastructure.Credentials;
using System.Collections.Generic;
using Foundations.HttpClient.Enums;
using Material.Infrastructure;

namespace Material.Infrastructure.ProtectedResources
{     
    /// <summary>
    /// Rescuetime API 0
    /// </summary>
    [CredentialType(typeof(OAuth2Credentials))]
	public partial class Rescuetime : OAuth2ResourceProvider              
	{
        public override Uri AuthorizationUrl => new Uri("https://www.rescuetime.com/oauth/authorize");
        public override Uri TokenUrl => new Uri("https://www.rescuetime.com/oauth/token");
        public override List<String> AvailableScopes => new List<String> { "time_data" };
        public override List<ResponseTypeEnum> Flows => new List<ResponseTypeEnum> { ResponseTypeEnum.Code };
        public override String TokenName => "access_token";
	}
}
