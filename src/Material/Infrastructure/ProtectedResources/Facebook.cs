using Material.Metadata;
using System;
using Material.Infrastructure.Credentials;
using System.Collections.Generic;
using Foundations.HttpClient.Enums;
using Material.Infrastructure;

namespace Material.Infrastructure.ProtectedResources
{     
    /// <summary>
    /// Facebook Graph API v2.7
    /// </summary>
    [CredentialType(typeof(OAuth2Credentials))]
	public partial class Facebook : OAuth2ResourceProvider              
	{
        public override Uri AuthorizationUrl => new Uri("https://www.facebook.com/dialog/oauth");
        public override Uri TokenUrl => new Uri("https://graph.facebook.com/oauth/access_token");
        public override List<String> AvailableScopes => new List<String> { "user_events", "user_likes", "user_friends", "user_posts" };
        public override List<ResponseTypeEnum> Flows => new List<ResponseTypeEnum> { ResponseTypeEnum.Code, ResponseTypeEnum.Token };
        public override String TokenName => "access_token";
        public override Char ScopeDelimiter => ',';
	}
}
