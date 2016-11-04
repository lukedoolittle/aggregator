using System;
using System.Collections.Generic;
using Foundations.HttpClient.Cryptography;
using Foundations.HttpClient.Enums;
using Material.Infrastructure.Credentials;
using Quantfabric.Test.Integration;
using Quantfabric.Test.OAuthServer;
using Quantfabric.Test.OAuthServer.Builders;

namespace Quantfabric.Test.Material.OAuthServer.Builders
{
    public class OAuth2CodeCredentialBuilder : 
        ICredentialBuilder<OAuth2Credentials, OAuth2AuthorizationRequest>
    {
        private readonly IDictionary<string, List<OAuth2Token>> _tokens;

        public OAuth2CodeCredentialBuilder(
            IDictionary<string, List<OAuth2Token>> tokens)
        {
            _tokens = tokens;
        }

        public OAuth2Credentials BuildCredentials(
            OAuth2AuthorizationRequest request)
        {
            if (request.ResponseType != OAuth2ResponseType.Code)
            {
                throw new Exception();
            }

            var code = new CryptoStringGenerator()
                .CreateRandomString(
                    32,
                    CryptoStringType.Base64Alphanumeric);

            _tokens[request.ClientId].Add(
                new OAuth2Token(
                    code, 
                    request.Scope));

            var credentials = new OAuth2Credentials();
            credentials.SetPropertyValue("Code", code);

            return credentials;
        }
    }
}
