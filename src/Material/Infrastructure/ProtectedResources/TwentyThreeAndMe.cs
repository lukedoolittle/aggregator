using Material.Metadata;
using System;
using Material.Infrastructure.Credentials;
using System.Collections.Generic;
using Foundations.HttpClient.Enums;
using Material.Infrastructure;

namespace Material.Infrastructure.ProtectedResources
{     
    /// <summary>
    /// 23andMe API 1
    /// </summary>
    [CredentialType(typeof(OAuth2Credentials))]
	public partial class TwentyThreeAndMe : OAuth2ResourceProvider              
	{
        public override Uri AuthorizationUrl => new Uri("https://api.23andme.com/authorize/");
        public override Uri TokenUrl => new Uri("https://api.23andme.com/token/");
        public override List<String> AvailableScopes => new List<String> { "basic", "genomes", "email" };
        public override List<ResponseTypeEnum> Flows => new List<ResponseTypeEnum> { ResponseTypeEnum.Code };
        public override String TokenName => "Bearer";
	}
}
