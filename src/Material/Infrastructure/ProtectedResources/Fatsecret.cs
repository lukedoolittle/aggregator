using Material.Metadata;
using System;
using Material.Infrastructure.Credentials;
using Material.Enums;
using Material.Infrastructure;

namespace Material.Infrastructure.ProtectedResources
{     
    /// <summary>
    /// Fatsecret Platform API 
    /// </summary>
    [CredentialType(typeof(OAuth1Credentials))]
	public partial class Fatsecret : OAuth1ResourceProvider              
	{
        public override Uri RequestUrl => new Uri("http://www.fatsecret.com/oauth/request_token");
        public override Uri AuthorizationUrl => new Uri("http://www.fatsecret.com/oauth/authorize");
        public override Uri TokenUrl => new Uri("http://www.fatsecret.com/oauth/access_token");
        public override OAuthParameterTypeEnum ParameterType => OAuthParameterTypeEnum.Querystring;
	}
}
