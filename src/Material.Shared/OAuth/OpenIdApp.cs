using System.Security;
using System.Threading.Tasks;
using Foundations.HttpClient.Enums;
using Material.Enums;
using Material.Infrastructure;
using Material.Infrastructure.Credentials;
using Material.OAuth.Authentication;

namespace Material.OAuth
{
    public class OpenIdApp<TResourceProvider>
        where TResourceProvider : OAuth2ResourceProvider, new()
    {
        private readonly OAuth2App<TResourceProvider> _app;
        private readonly TResourceProvider _provider;

        /// <summary>
        /// Authorize a resource owner using the OAuth2 workflow
        /// </summary>
        /// <param name="clientId">The application's client Id</param>
        /// <param name="callbackUrl">The application's registered callback url</param>
        /// <param name="provider">The provider to authenticate with (CUSTOM IMPLEMENTAIONS ONLY)</param>
        /// <param name="browserType">The type of browser interface used for the workflow</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1054:UriParametersShouldNotBeStrings", MessageId = "1#")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1026:DefaultParametersShouldNotBeUsed")]
        public OpenIdApp(
            string clientId,
            string callbackUrl,
            TResourceProvider provider,
#if __WINDOWS__
            AuthorizationInterface browserType = AuthorizationInterface.Dedicated
#else
            AuthorizationInterface browserType = AuthorizationInterface.Embedded
#endif
            )
        {
            _provider = provider;
             _app = new OAuth2App<TResourceProvider>(
                clientId,
                callbackUrl,
                provider,
                browserType);
            _app.AddScope("openid");
        }

        /// <summary>
        /// Authorize a resource owner using the OAuth2 OpenId Connect workflow
        /// </summary>
        /// <param name="clientId">The application's client Id</param>
        /// <param name="callbackUrl">The application's registered callback url</param>
        /// <param name="browserType">The type of browser interface used for the workflow</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1054:UriParametersShouldNotBeStrings", MessageId = "1#")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1026:DefaultParametersShouldNotBeUsed")]
        public OpenIdApp(
            string clientId,
            string callbackUrl,
#if __WINDOWS__
            AuthorizationInterface browserType = AuthorizationInterface.Dedicated
#else
            AuthorizationInterface browserType = AuthorizationInterface.Embedded
#endif
            ) :
            this(
                clientId,
                callbackUrl,
                new TResourceProvider(),
                browserType)
        { }

        /// <summary>
        /// Authorize a resource owner using the OAuth2 code workflow
        /// </summary>
        /// <param name="clientSecret">The client secret for the application</param>
        /// <returns>Valid OAuth2 credentials</returns>
        public async Task<JsonWebToken> GetCredentialsAsync(
            string clientSecret)
        {
            var credentials = await _app
                .GetCredentialsAsync(clientSecret)
                .ConfigureAwait(false);

            return ExtractAndValidateAuthenticationToken(credentials);
        }

        /// <summary>
        /// Authorize a resource owner using the OAuth2 token workflow
        /// </summary>
        /// <returns>Valid OAuth2 credentials</returns>
        public async Task<JsonWebToken> GetCredentialsAsync()
        {
            var credentials = await _app.GetCredentialsAsync(
                    OAuth2ResponseType.IdTokenToken)
                .ConfigureAwait(false);

            return ExtractAndValidateAuthenticationToken(credentials);
        }

        private JsonWebToken ExtractAndValidateAuthenticationToken(
            OAuth2Credentials credentials)
        {
            //TODO: potentially add more validators here

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
