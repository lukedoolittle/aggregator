using Material.Metadata;
using System;
using Material.Infrastructure.Credentials;
using System.Collections.Generic;
using Material.Enums;
using Material.Infrastructure;

namespace Material.Infrastructure.ProtectedResources
{     
    /// <summary>
    /// Linkedin API 1
    /// </summary>
    [CredentialType(typeof(OAuth2Credentials))]
	public partial class LinkedIn : OAuth2ResourceProvider              
	{
        public override Uri AuthorizationUrl => new Uri("https://www.linkedin.com/uas/oauth2/authorization");
        public override Uri TokenUrl => new Uri("https://www.linkedin.com/uas/oauth2/accessToken");
        public override List<String> AvailableScopes => new List<String> { "r_basicprofile", "w_share", "r_emailaddress", "rw_company_admin" };
        public override List<ResponseTypeEnum> Flows => new List<ResponseTypeEnum> { ResponseTypeEnum.Code };
        public override String TokenName => "Bearer";
        public override Char ScopeDelimiter => ' ';
	}
}
