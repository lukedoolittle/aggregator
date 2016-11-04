using System;
using Material.Infrastructure.Credentials;
using Quantfabric.Test.Material.OAuthServer.Requests;

namespace Quantfabric.Test.Material.OAuthServer.Builders
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
