using System;
using Material.Infrastructure.Credentials;

namespace Quantfabric.Test.OAuthServer.Builders
{
    public class OAuth2RefreshTokenBuilder :
        ICredentialBuilder<OAuth2Credentials, OAuth2TokenRequest>
    {
        public OAuth2Credentials BuildCredentials(OAuth2TokenRequest request)
        {
            //CheckRefreshToken(request.RefreshToken);
            throw new NotImplementedException();
        }
    }
}
