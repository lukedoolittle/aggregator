using System;
using Material.Infrastructure.Credentials;

namespace Quantfabric.Test.OAuthServer.Builders
{
    public class OAuth2JwtBuilder
        : ICredentialBuilder<OAuth2Credentials, OAuth2TokenRequest>
    {
        public OAuth2Credentials BuildCredentials(OAuth2TokenRequest request)
        {
            throw new NotImplementedException();
            //CheckJsonWebToken(request.JsonWebToken);
        }
    }
}
