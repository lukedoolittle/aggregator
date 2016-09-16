using Material.Metadata;
using System;
using Material.Infrastructure.Credentials;
using System.Collections.Generic;
using Foundations.HttpClient.Enums;
using Material.Infrastructure;

namespace Material.Infrastructure.ProtectedResources
{     
    /// <summary>
    /// Instagram Platform API 1
    /// </summary>
    [CredentialType(typeof(OAuth2Credentials))]
	public partial class Instagram : OAuth2ResourceProvider              
	{
        public override Uri AuthorizationUrl => new Uri("https://api.instagram.com/oauth/authorize/");
        public override List<String> AvailableScopes => new List<String> { "basic", "public_content", "follower_list", "comments", "relationships", "likes" };
        public override List<ResponseTypeEnum> Flows => new List<ResponseTypeEnum> { ResponseTypeEnum.Code, ResponseTypeEnum.Token };
        public override List<GrantTypeEnum> GrantTypes => new List<GrantTypeEnum> { GrantTypeEnum.AuthCode };
        public override String TokenName => "access_token";
        public override Uri TokenUrl => new Uri("https://api.instagram.com/oauth/access_token");
	}
}
