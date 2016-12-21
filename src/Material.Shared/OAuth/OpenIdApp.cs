using System;
using System.Security;
using System.Threading.Tasks;
using Foundations.Extensions;
using Foundations.HttpClient.Enums;
using Material.Contracts;
using Material.Enums;
using Material.Infrastructure;
using Material.Infrastructure.Credentials;
using Material.OAuth.Authentication;
using Material.OAuth.Security;
using Material.OAuth.Workflow;

namespace Material.OAuth
{
    public class OpenIdApp<TResourceProvider>
        where TResourceProvider : OpenIdResourceProvider, new()
    {
        private readonly OAuth2AppBase<TResourceProvider> _app;
        private readonly TResourceProvider _provider;
        private readonly string _clientId;
        private readonly IOAuthSecurityStrategy _securityStrategy;
        private readonly string _userId;
        private readonly AuthorizationInterface _browserType;

        /// <summary>
        /// Authorize a resource owner using the OAuth2 workflow
        /// </summary>
        /// <param name="clientId">The application's client Id</param>
        /// <param name="callbackUrl">The application's registered callback url</param>
        /// <param name="provider">The provider to authenticate with (CUSTOM IMPLEMENTAIONS ONLY)</param>
        /// <param name="browserType">The type of browser interface used for the workflow</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1054:UriParametersShouldNotBeStrings", MessageId = "1#")]
        public OpenIdApp(
            string clientId,
            string callbackUrl,
            TResourceProvider provider,
            AuthorizationInterface browserType)
        {
#if __WINDOWS__
            _browserType = browserType;
#else
            _browserType = new AuthenticationUISelector(
                    Framework.Platform.Current.CanProvideSecureBrowsing)
                .GetOptimalOAuth2Interface(
                    provider,
                    browserType);
#endif
            _clientId = clientId;
            _provider = provider;
            _userId = Guid.NewGuid().ToString();

            _securityStrategy = new OAuthSecurityStrategy(
                new InMemoryCryptographicParameterRepository(),
                TimeSpan.FromMinutes(
                    OAuthConfiguration.SecurityParameterTimeoutInMinutes));

            _app = new OAuth2AppBase<TResourceProvider>(
                clientId,
                new Uri(callbackUrl),
#if __FORMS__
                    Xamarin.Forms.DependencyService.Get<IOAuthAuthorizerUIFactory>(),
#else
                    new OAuthAuthorizerUIFactory(),
#endif
                provider,
                _browserType,
                _userId);

            _app.AddScope("openid");
        }

        /// <summary>
        /// Authorize a resource owner using the OAuth2 workflow
        /// </summary>
        /// <param name="clientId">The application's client Id</param>
        /// <param name="callbackUrl">The application's registered callback url</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1054:UriParametersShouldNotBeStrings", MessageId = "1#")]
        public OpenIdApp(
            string clientId,
            string callbackUrl) : 
                this(
                    clientId, 
                    callbackUrl, 
                    AuthorizationInterface.NotSpecified)
        {}

        /// <summary>
        /// Authorize a resource owner using the OAuth2 workflow
        /// </summary>
        /// <param name="clientId">The application's client Id</param>
        /// <param name="callbackUrl">The application's registered callback url</param>
        /// <param name="provider">The provider to authenticate with (CUSTOM IMPLEMENTAIONS ONLY)</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1054:UriParametersShouldNotBeStrings",
            MessageId = "1#")]
        public OpenIdApp(
            string clientId,
            string callbackUrl,
            TResourceProvider provider) : 
                this(
                    clientId, 
                    callbackUrl, 
                    provider, 
                    AuthorizationInterface.NotSpecified)
        {}

        /// <summary>
        /// Authorize a resource owner using the OAuth2 workflow
        /// </summary>
        /// <param name="clientId">The application's client Id</param>
        /// <param name="callbackUrl">The application's registered callback url</param>
        /// <param name="browserType">The type of browser interface used for the workflow</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1054:UriParametersShouldNotBeStrings", MessageId = "1#")]
        public OpenIdApp(
            string clientId,
            string callbackUrl,
            AuthorizationInterface browserType) :
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
        public async Task<JsonWebToken> GetWebTokenAsync(
            string clientSecret)
        {
            var token = await _app
                .GetIdTokenAsync(clientSecret)
                .ConfigureAwait(false);

            return ValidateToken(token);
        }

        /// <summary>
        /// Authorize a resource owner using the OAuth2 token workflow
        /// </summary>
        /// <param name="response">The OAuth2 response type</param>
        /// <returns>Valid OAuth2 credentials</returns>
        public async Task<JsonWebToken> GetWebTokenAsync(
            OAuth2ResponseType response)
        {
            var token = await _app
                .GetIdTokenAsync()
                .ConfigureAwait(false);

            return ValidateToken(token);
        }

        /// <summary>
        /// Authorize a resource owner using the OAuth2 token workflow
        /// </summary>
        /// <returns>Valid OAuth2 credentials</returns>
        public Task<JsonWebToken> GetWebTokenAsync()
        {
            return GetWebTokenAsync(OAuth2ResponseType.IdTokenToken);
        }

        private JsonWebToken ValidateToken(JsonWebToken token)
        {
            new CompositeJsonWebTokenAuthenticationValidator()
                .AddValidator(new JsonWebTokenAlgorithmValidator())
                .AddValidator(new JsonWebTokenExpirationValidator())
                .AddValidator(new JsonWebTokenAudienceValidator(
                    _clientId))
                .AddValidator(new JsonWebTokenIssuerValidator(
                    _provider.ValidIssuers))
                .AddValidator(new JsonWebTokenNonceValidator(
                    _securityStrategy, 
                    _userId))
                .AddValidator(new DiscoveryJsonWebTokenSignatureValidator(
                    _provider.OpenIdDiscoveryUrl))
                .ThrowIfInvalid()
                .IsTokenValid(token);

            return token;
        }
    }
}
