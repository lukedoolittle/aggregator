using System;
using Material.Domain.Credentials;
using System.Collections.Generic;
using System.Linq;
using Material.HttpClient.Cryptography;
using Material.HttpClient.Cryptography.Enums;
using Quantfabric.Test.Material.OAuthServer.Requests;
using Quantfabric.Test.Material.OAuthServer.Tokens;

namespace Quantfabric.Test.Material.OAuthServer.Builders
{
    public class OAuth1AccessTokenCredentialBuilder : 
        ICredentialBuilder<OAuth1Credentials, OAuth1Request>
    {
        private readonly IDictionary<string, List<OAuth1Token>> _tokens;
        private readonly bool _createUserId;

        public OAuth1AccessTokenCredentialBuilder(
            IDictionary<string, List<OAuth1Token>> tokens, 
            bool createUserId)
        {
            _tokens = tokens;
            _createUserId = createUserId;
        }

        public OAuth1Credentials BuildCredentials(OAuth1Request request)
        {
            var token = _tokens[request.OAuthToken]
                .SingleOrDefault(t => 
                    t.Verifier == request.Verifier && 
                    t.AreTemporaryCredentials);

            if (token == null)
            {
                throw new Exception();
            }

            var stringGenerator = new CryptoStringGenerator();

            var oauthToken = stringGenerator
                .CreateRandomString(
                    32,
                    CryptoStringType.Base64Alphanumeric);

            var oauthSecret = stringGenerator
                .CreateRandomString(
                    32,
                    CryptoStringType.Base64Alphanumeric);

            _tokens[request.OAuthToken].Remove(token);
            _tokens.Add(oauthToken, new List<OAuth1Token>());
            _tokens[oauthToken].Add(
                new OAuth1Token(
                    oauthToken,
                    oauthSecret,
                    null,
                    false));

            var credentials = new OAuth1Credentials();
            credentials.SetPropertyValue(
                nameof(credentials.OAuthToken),
                oauthToken);
            credentials.SetPropertyValue(
                nameof(credentials.OAuthSecret),
                oauthSecret);

            if (_createUserId)
            {
                var userId = stringGenerator
                    .CreateRandomString(
                        10,
                        CryptoStringType.Base64Alphanumeric);

                credentials.SetMemberValue(
                    "_userId1",
                    userId);
            }

            return credentials;
        }
    }
}
