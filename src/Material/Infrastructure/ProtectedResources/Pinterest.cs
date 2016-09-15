using Material.Metadata;
using System;
using Material.Infrastructure.Credentials;
using System.Collections.Generic;
using Foundations.HttpClient.Enums;
using Material.Infrastructure;

namespace Material.Infrastructure.ProtectedResources
{     
    /// <summary>
    /// Pinterest API 1
    /// </summary>
    [CredentialType(typeof(OAuth2Credentials))]
	public partial class Pinterest : OAuth2ResourceProvider              
	{
        public override Uri AuthorizationUrl => new Uri("https://api.pinterest.com/oauth/");
        public override List<String> AvailableScopes => new List<String> { "read_public", "write_public", "read_relationships", "write_relationships" };
        public override List<ResponseTypeEnum> Flows => new List<ResponseTypeEnum> { ResponseTypeEnum.Code };
        public override List<GrantTypeEnum> GrantTypes => new List<GrantTypeEnum> { GrantTypeEnum.AuthCode };
        public override String TokenName => "access_token";
        public override Char ScopeDelimiter => ',';
        public override Uri TokenUrl => new Uri("https://api.pinterest.com/v1/oauth/token");
	}
}
