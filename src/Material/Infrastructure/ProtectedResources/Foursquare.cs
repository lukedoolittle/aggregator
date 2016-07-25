using Material.Metadata;
using System;
using Material.Infrastructure.Credentials;
using System.Collections.Generic;
using Material.Enums;
using Material.Infrastructure;

namespace Material.Infrastructure.ProtectedResources
{     
    /// <summary>
    /// Foursquare and Swarm API 2
    /// </summary>
    [CredentialType(typeof(OAuth2Credentials))]
	public partial class Foursquare : OAuth2ResourceProvider              
	{
        public override Uri AuthorizationUrl => new Uri("https://foursquare.com/oauth2/authorize");
        public override Uri TokenUrl => new Uri("https://foursquare.com/oauth2/access_token");
        public override List<String> AvailableScopes => new List<String>();
        public override List<ResponseTypeEnum> Flows => new List<ResponseTypeEnum> { ResponseTypeEnum.Code, ResponseTypeEnum.Token };
        public override String TokenName => "oauth_token";
	}
}
