using System;
using System.Collections.Generic;
using System.Linq;
using Foundations.HttpClient.Cryptography;
using Foundations.HttpClient.Cryptography.Enums;
using Material.Infrastructure.Credentials;
using Quantfabric.Test.Integration;
using Quantfabric.Test.Material.OAuthServer.Requests;
using Quantfabric.Test.Material.OAuthServer.Tokens;

namespace Quantfabric.Test.Material.OAuthServer.Builders
{
    public class OAuth1AuthenticationCredentialBuilder : 
        ICredentialBuilder<OAuth1Credentials, OAuth1Request>
    {
        private readonly IDictionary<string, List<OAuth1Token>> _tokens;

        public OAuth1AuthenticationCredentialBuilder(
            IDictionary<string, List<OAuth1Token>> tokens)
        {
            _tokens = tokens;
        }

        public OAuth1Credentials BuildCredentials(OAuth1Request request)
        {
            var token = _tokens[request.OAuthToken].SingleOrDefault();

            if (token == null)
            {
                throw new Exception();
            }

            var verifier = new CryptoStringGenerator()
                .CreateRandomString(
                    32,
                    CryptoStringType.Base64Alphanumeric);

            token.SetVerifier(verifier);

            var credentials = new OAuth1Credentials();
            credentials.SetPropertyValue(
                nameof(credentials.Verifier), 
                verifier);
            credentials.SetPropertyValue(
                nameof(credentials.OAuthToken),
                request.OAuthToken);

            return credentials;
        }
    }
}
