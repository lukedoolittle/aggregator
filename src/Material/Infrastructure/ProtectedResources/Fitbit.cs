using Material.Metadata;
using System;
using Material.Infrastructure.Credentials;
using System.Collections.Generic;
using Material.Enums;
using Material.Infrastructure;

namespace Material.Infrastructure.ProtectedResources
{     
    /// <summary>
    /// Fitbit API 1
    /// </summary>
    [CredentialType(typeof(OAuth2Credentials))]
	public partial class Fitbit : OAuth2ResourceProvider              
	{
        public override Uri AuthorizationUrl => new Uri("https://www.fitbit.com/oauth2/authorize");
        public override Uri TokenUrl => new Uri("https://api.fitbit.com/oauth2/token");
        public override List<String> AvailableScopes => new List<String> { "sleep", "activity", "heartrate", "profile" };
        public override List<ResponseTypeEnum> Flows => new List<ResponseTypeEnum> { ResponseTypeEnum.Code, ResponseTypeEnum.Token };
        public override String TokenName => "Bearer";
	}
}
