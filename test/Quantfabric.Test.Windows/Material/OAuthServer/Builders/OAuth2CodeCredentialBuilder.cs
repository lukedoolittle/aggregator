using System;
using Material.Domain.Credentials;
using System.Collections.Generic;
using Material.Framework.Enums;
using Material.HttpClient.Cryptography;
using Material.HttpClient.Cryptography.Enums;
using Quantfabric.Test.Material.OAuthServer.Requests;
using Quantfabric.Test.Material.OAuthServer.Tokens;

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
