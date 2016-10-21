using System;
using System.Collections.Generic;
using Foundations.Cryptography;
using Foundations.Cryptography.StringCreation;
using Foundations.HttpClient.Enums;
using Material.Infrastructure.Credentials;
using Quantfabric.Test.Integration;

namespace Quantfabric.Test.OAuthServer.Builders
{
    public class OAuth2TokenCredentialBuilder : 
        ICredentialBuilder<OAuth2Credentials, OAuth2AuthorizationRequest>
    {
        private readonly IDictionary<string, List<OAuth2Token>> _tokens;
        private readonly int _msUntilExpiry;

        public OAuth2TokenCredentialBuilder(
            IDictionary<string, List<OAuth2Token>> tokens,
            int msUntilExpiry = 0)
        {
            _tokens = tokens;
            _msUntilExpiry = msUntilExpiry;
        }

        public OAuth2Credentials BuildCredentials(
            OAuth2AuthorizationRequest request)
        {
            if (request.ResponseType != OAuth2ResponseType.Token)
            {
                throw new Exception();
            }

            var accessToken = new CryptoStringGenerator()
                .CreateRandomString(
                    32,
                    CryptoStringType.Base64Alphanumeric);

            var token = new OAuth2Token(request.Scope);
            token.SetToken(accessToken);
            _tokens[request.ClientId].Add(token);

            var credentials = new OAuth2Credentials();
            credentials.SetMemberValue("_accessToken", accessToken);
            if (_msUntilExpiry > 0)
            {
                credentials.SetMemberValue("_expiresIn", _msUntilExpiry.ToString());
            }

            return credentials;
        }
    }
}
