using Material.Metadata;
using System;
using Material.Infrastructure.Credentials;
using Material.Enums;
using Material.Infrastructure;

namespace Material.Infrastructure.ProtectedResources
{     
    /// <summary>
    /// Withings API 1.0.1
    /// </summary>
    [CredentialType(typeof(OAuth1Credentials))]
	public partial class Withings : OAuth1ResourceProvider              
	{
        public override Uri RequestUrl => new Uri("https://oauth.withings.com/account/request_token");
        public override Uri AuthorizationUrl => new Uri("https://oauth.withings.com/account/authorize");
        public override Uri TokenUrl => new Uri("https://oauth.withings.com/account/access_token");
        public override OAuthParameterTypeEnum ParameterType => OAuthParameterTypeEnum.Querystring;
	}
}
