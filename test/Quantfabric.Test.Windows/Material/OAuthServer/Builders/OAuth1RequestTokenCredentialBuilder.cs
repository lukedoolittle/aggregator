using System.Collections.Generic;
using Foundations.HttpClient.Cryptography;
using Foundations.HttpClient.Cryptography.Enums;
using Material.Infrastructure.Credentials;
using Quantfabric.Test.Integration;
using Quantfabric.Test.Material.OAuthServer.Requests;
using Quantfabric.Test.Material.OAuthServer.Tokens;

namespace Quantfabric.Test.Material.OAuthServer.Builders
{
    public class OAuth1RequestTokenCredentialBuilder : 
        ICredentialBuilder<OAuth1Credentials, OAuth1Request>
    {
        private readonly IDictionary<string, List<OAuth1Token>> _tokens;

        public OAuth1RequestTokenCredentialBuilder(
            IDictionary<string, List<OAuth1Token>> tokens)
        {
            _tokens = tokens;
        }

        public OAuth1Credentials BuildCredentials(OAuth1Request request)
        {
            var stringGenerator = new CryptoStringGenerator();

            var oauthToken = stringGenerator
                .CreateRandomString(
                    32,
                    CryptoStringType.Base64Alphanumeric);

            var oauthSecret = stringGenerator
                .CreateRandomString(
                    32,
                    CryptoStringType.Base64Alphanumeric);

            _tokens.Add(oauthToken, new List<OAuth1Token>());
            _tokens[oauthToken].Add(
                new OAuth1Token(
                    oauthToken,
                    oauthSecret,
                    request.RedirectUri,
                    true));

            var credentials = new OAuth1Credentials();
            credentials.SetPropertyValue(
                nameof(credentials.OAuthToken), 
                oauthToken);
            credentials.SetPropertyValue(
                nameof(credentials.OAuthSecret),
                oauthSecret);
            credentials.SetPropertyValue(
                nameof(credentials.CallbackConfirmed),
                true);

            return credentials;
        }
    }
}
