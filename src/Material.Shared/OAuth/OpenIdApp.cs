using System;
using System.Security;
using System.Threading.Tasks;
using Foundations.Extensions;
using Foundations.HttpClient.Cryptography;
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
        private readonly AuthorizationInterface _browserType;
        private readonly ICryptoStringGenerator _idGenerator;

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
            var redirectUri = new Uri(callbackUrl);

#if __WINDOWS__
            _browserType = browserType;
#else
            _browserType = new AuthenticationUISelector(
                    Framework.Platform.Current.CanProvideSecureBrowsing)
                .GetOptimalOAuth2Interface(
                    provider,
                    browserType,
                    redirectUri);
#endif
            _clientId = clientId;
            _provider = provider;

            _securityStrategy = new OAuthSecurityStrategy(
                new InMemoryCryptographicParameterRepository(),
                TimeSpan.FromMinutes(
                    OAuthConfiguration.SecurityParameterTimeoutInMinutes));

            _app = new OAuth2AppBase<TResourceProvider>(
                clientId,
                redirectUri,
#if __FORMS__
                    Xamarin.Forms.DependencyService.Get<IOAuthAuthorizerUIFactory>(),
#else
                    new OAuthAuthorizerUIFactory(),
#endif
                provider,
                _browserType);

            _idGenerator = new CryptoStringGenerator();
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
        public Task<JsonWebToken> GetWebTokenAsync(
            string clientSecret)
        {
            _app.AddScope("openid");

            var requestId = _idGenerator.CreateRandomString();

            return _app
                .GetIdTokenAsync(
                    clientSecret,
                    CreateValidator(requestId),
                    requestId);
        }

        /// <summary>
        /// Authorize a resource owner using the OAuth2 token workflow
        /// </summary>
        /// <returns>Valid OAuth2 credentials</returns>
        public Task<JsonWebToken> GetWebTokenAsync()
        {
            _app.AddScope("openid");

            var requestId = _idGenerator.CreateRandomString();

            return _app
                .GetIdTokenAsync(
                CreateValidator(requestId),
                requestId);
        }

        private IJsonWebTokenAuthenticationValidator CreateValidator(
            string requestId)
        {
            return new CompositeJsonWebTokenAuthenticationValidator()
                .AddValidator(new JsonWebTokenAlgorithmValidator())
                .AddValidator(new JsonWebTokenExpirationValidator())
                .AddValidator(new JsonWebTokenAudienceValidator(
                    _clientId))
                .AddValidator(new JsonWebTokenIssuerValidator(
                    _provider.ValidIssuers))
                .AddValidator(new JsonWebTokenNonceValidator(
                    _securityStrategy,
                    requestId))
                .AddValidator(new DiscoveryJsonWebTokenSignatureValidator(
                    _provider.OpenIdDiscoveryUrl))
                .ThrowIfInvalid();
        }
    }
}
