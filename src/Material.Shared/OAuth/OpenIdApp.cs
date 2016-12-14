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
using Material.OAuth.Authorization;
using Material.OAuth.Callback;
using Material.OAuth.Facade;
using Material.OAuth.Security;
using Material.OAuth.Workflow;

namespace Material.OAuth
{
    public class OpenIdApp<TResourceProvider>
        where TResourceProvider : OpenIdResourceProvider, new()
    {
        private readonly OAuth2AppBase<TResourceProvider> _app;
        private readonly OAuth2CallbackHandler _callbackHandler;
        private readonly TResourceProvider _provider;
        private readonly string _clientId;
        private readonly Uri _callbackUri;
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
            _browserType = new AuthenticationUISelector(
                    Framework.Platform.Current.CanProvideSecureBrowsing)
                .GetOptimalOAuth2Interface(
                    provider,
                    browserType);
            _clientId = clientId;
            _provider = provider;
            _callbackUri = new Uri(callbackUrl);
            _userId = Guid.NewGuid().ToString();

            _securityStrategy = new OAuthSecurityStrategy(
                new InMemoryCryptographicParameterRepository(),
                TimeSpan.FromMinutes(
                    OAuthConfiguration.SecurityParameterTimeoutInMinutes));

            _callbackHandler = new OAuth2CallbackHandler(
                _securityStrategy,
                OAuth2Parameter.State.EnumToString());

            _app = new OAuth2AppBase<TResourceProvider>(
                new Uri(callbackUrl),
#if __FORMS__
                    Xamarin.Forms.DependencyService.Get<IOAuthAuthorizerUIFactory>(),
#else
                    new OAuthAuthorizerUIFactory(),
#endif
                provider,
                browserType,
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
            var facade = new OpenIdCodeAuthorizationFacade(
                _provider,
                _clientId,
                _callbackUri,
                new OAuth2AuthorizationAdapter(),
                _securityStrategy);

            var credentials = await _app.GetCredentialsAsync(
                    clientSecret,
                    OAuth2FlowType.AccessCode,
                    OAuth2ResponseType.Code,
                    facade,
                    _callbackHandler)
                .ConfigureAwait(false);

            return GetTokenFromCredentials(credentials);
        }

        /// <summary>
        /// Authorize a resource owner using the OAuth2 token workflow
        /// </summary>
        /// <param name="response">The OAuth2 response type</param>
        /// <returns>Valid OAuth2 credentials</returns>
        public async Task<JsonWebToken> GetWebTokenAsync(
            OAuth2ResponseType response)
        {
#if !__WINDOWS__
            //This is sort of a bizarre hack: Google requires that you go through the
            //code workflow with a mobile device even if you don't have a client secret
            if (_browserType == AuthorizationInterface.Dedicated &&
                typeof(TResourceProvider) == typeof(Infrastructure.ProtectedResources.Google))
            {
                var codeFacade = new OpenIdCodeAuthorizationFacade(
                    _provider,
                    _clientId,
                    _callbackUri,
                    new OAuth2AuthorizationAdapter(),
                    _securityStrategy);

                var codeCredentials = await _app.GetCredentialsAsync(
                        null,
                        OAuth2FlowType.AccessCode, 
                        OAuth2ResponseType.Code,
                        codeFacade,
                        _callbackHandler)
                    .ConfigureAwait(false);

                return GetTokenFromCredentials(codeCredentials);
            }
#endif
            var tokenFacade = new OpenIdTokenAuthorizationFacade(
                _provider,
                _clientId,
                _callbackUri,
                new OAuth2AuthorizationAdapter(),
                _securityStrategy);

            var credentials = await _app.GetCredentialsAsync(
                    OAuth2FlowType.Implicit,
                    response,
                    tokenFacade,
                    _callbackHandler)
                .ConfigureAwait(false);

            return GetTokenFromCredentials(credentials);
        }

        /// <summary>
        /// Authorize a resource owner using the OAuth2 token workflow
        /// </summary>
        /// <returns>Valid OAuth2 credentials</returns>
        public Task<JsonWebToken> GetWebTokenAsync()
        {
            return GetWebTokenAsync(OAuth2ResponseType.IdTokenToken);
        }

        private JsonWebToken GetTokenFromCredentials(OAuth2Credentials credentials)
        {
            var nonce = _securityStrategy.CreateOrGetSecureParameter(
                _userId,
                OAuth2Parameter.Nonce.EnumToString());

            var validator = new CompositeJsonWebTokenAuthenticationValidator()
                .AddValidator(new JsonWebTokenAlgorithmValidator())
                .AddValidator(new JsonWebTokenExpirationValidator())
                .AddValidator(new JsonWebTokenAudienceValidator(_clientId))
                .AddValidator(new JsonWebTokenIssuerValidator(_provider.ValidIssuers))
                .AddValidator(new JsonWebTokenNonceValidator(nonce))
                .AddValidator(new DiscoveryJsonWebTokenSignatureValidator(_provider.OpenIdDiscoveryUrl));

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
