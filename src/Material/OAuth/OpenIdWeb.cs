using System;
using System.Security;
using System.Threading.Tasks;
using Material.Contracts;
using Material.Infrastructure;
using Material.Infrastructure.Credentials;
using Material.OAuth.Authentication;

namespace Material.OAuth
{
    public class OpenIdWeb<TResourceProvider>
        where TResourceProvider : OpenIdResourceProvider, new()
    {
        private readonly OAuth2Web<TResourceProvider> _web;
        private readonly TResourceProvider _provider;

        /// <summary>
        /// Authenticate a resource owner using the OpenId Connect workflow with default security strategy
        /// </summary>
        /// <param name="clientId">The application's client Id</param>
        /// <param name="clientSecret">The application's client secret</param>
        /// <param name="callbackUrl">The application's registered callback url</param>
        /// <param name="strategy">The security strategy to use for "state" handling</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1054:UriParametersShouldNotBeStrings", MessageId = "2#")]
        public OpenIdWeb(
            string clientId,
            string clientSecret,
            string callbackUrl,
            IOAuthSecurityStrategy strategy)
        {
            _provider = new TResourceProvider();
            _web = new OAuth2Web<TResourceProvider>(
                clientId, 
                clientSecret, 
                callbackUrl, 
                strategy,
                _provider);
        }

        /// <summary>
        /// Authenticate a resource owner using the OpenId Connect workflow with default security strategy
        /// </summary>
        /// <param name="clientId">The application's client Id</param>
        /// <param name="clientSecret">The application's client secret</param>
        /// <param name="callbackUrl">The application's registered callback url</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1054:UriParametersShouldNotBeStrings",
            MessageId = "2#")]
        public OpenIdWeb(
            string clientId,
            string clientSecret,
            string callbackUrl)
        {
            _provider = new TResourceProvider();
            _web = new OAuth2Web<TResourceProvider>(
                clientId,
                clientSecret,
                callbackUrl,
                _provider);
        }

        /// <summary>
        /// Gets the authorization uri for the Resource Owner to enter his/her credentials
        /// </summary>
        /// <param name="userId">Resource owner's Id</param>
        /// <returns>Authorization uri</returns>
        public Task<Uri> GetAuthorizationUriAsync(string userId)
        {
            _web.AddScope("openid");

            return _web.GetAuthorizationUriAsync(userId);
        }

        /// <summary>
        /// Exchanges callback uri for access token credentials
        /// </summary>
        /// <param name="userId">Resource owner's Id</param>
        /// <param name="responseUri">The received callback uri</param>
        /// <returns>Access token credentials</returns>
        public async Task<JsonWebToken> GetWebTokenAsync(
            Uri responseUri,
            string userId)
        {
            var credentials = await _web.GetAccessTokenAsync(
                    responseUri, 
                    userId)
                .ConfigureAwait(false);

            var validator = new CompositeJsonWebTokenAuthenticationValidator(
                new DiscoveryJsonWebTokenSignatureValidator(_provider.OpenIdDiscoveryUrl),
                new JsonWebTokenAlgorithmValidator(),
                new JsonWebTokenExpirationValidator());

            var token = credentials.IdToken;

            var tokenValidation = validator
                .IsTokenValid(token);

            if (!tokenValidation.IsTokenValid)
            {
                throw new SecurityException(
                    tokenValidation.Reason);
            }

            return token;
        }
    }
}
