using Material.Metadata;
using System;
using Material.Infrastructure.Credentials;
using System.Collections.Generic;
using Material.Enums;
using Material.Infrastructure;

namespace Material.Infrastructure.ProtectedResources
{     
    /// <summary>
    /// Google API 1
    /// </summary>
    [CredentialType(typeof(OAuth2Credentials))]
	public partial class Google : OAuth2ResourceProvider              
	{
        public override Uri AuthorizationUrl => new Uri("https://accounts.google.com/o/oauth2/auth");
        public override Uri TokenUrl => new Uri("https://accounts.google.com/o/oauth2/token");
        public override List<String> AvailableScopes => new List<String> { "https://www.googleapis.com/auth/gmail.readonly" };
        public override List<ResponseTypeEnum> Flows => new List<ResponseTypeEnum> { ResponseTypeEnum.Code, ResponseTypeEnum.Token };
        public override String TokenName => "Bearer";
	}
}
