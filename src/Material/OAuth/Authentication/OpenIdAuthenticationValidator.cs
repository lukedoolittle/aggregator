using System;
using System.Threading.Tasks;
using Foundations.Extensions;
using Foundations.HttpClient;
using Foundations.HttpClient.Cryptography.Discovery;
using Foundations.HttpClient.Extensions;
using Material.Infrastructure.Credentials;

namespace Material.OAuth.Authentication
{
    public class OpenIdAuthenticationValidator
    {
        private readonly AuthenticationValidator _validator;

        public OpenIdAuthenticationValidator(
            AuthenticationValidator validator)
        {
            _validator = validator;
        }

        public async Task<TokenValidationResult> IsTokenValid(
            JsonWebToken token,
            Uri openIdDiscoveryUrl)
        {
            var discoveryDocument = await new HttpRequestBuilder(openIdDiscoveryUrl.NonPath())
                .GetFrom(openIdDiscoveryUrl.AbsolutePath)
                .ResultAsync<OpenIdConnectDiscoveryDocument>()
                .ConfigureAwait(false);

            var keysUrl = new Uri(discoveryDocument.JsonWebKeysUri);

            var keys = await new HttpRequestBuilder(keysUrl.NonPath())
                .GetFrom(keysUrl.AbsolutePath)
                .ResultAsync<PublicKeyDiscoveryDocument>()
                .ConfigureAwait(false);

            //TODO: need to figure out which key we need, and then how to convert it into a CryptoKey

            return _validator.IsTokenValid(token, null);
        }
    }
}
