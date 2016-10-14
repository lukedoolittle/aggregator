using System;
using System.Threading.Tasks;
using Foundations.Extensions;
using Foundations.HttpClient.Enums;
using Material.Contracts;
using Material.Enums;
using Material.Infrastructure.Credentials;
using Material.Infrastructure.OAuth.Callback;
using Material.Infrastructure.OAuth.Template;

namespace Material.Infrastructure.OAuth
{
    public class OAuth2AppBase<TResourceProvider>
        where TResourceProvider : OAuth2ResourceProvider
    {
        private readonly Uri _callbackUri;
        private readonly AuthenticationInterfaceEnum _browserType;
        private readonly IOAuthAuthorizerUIFactory _uiFactory;
        private readonly IOAuthFacade<OAuth2Credentials> _oauthFacade;
        private readonly IOAuthSecurityStrategy _securityStrategy;
        protected readonly TResourceProvider _provider;

        public OAuth2AppBase(
            string clientId,
            string callbackUrl,
            IOAuthAuthorizerUIFactory uiFactory,
            TResourceProvider provider,
            AuthenticationInterfaceEnum browserType)
        {
            _callbackUri = new Uri(callbackUrl);
            _browserType = browserType;
            _provider = provider;
            _uiFactory = uiFactory;

            _securityStrategy = new OAuthSecurityStrategy(
                new InMemoryCryptographicParameterRepository(),
                TimeSpan.FromMinutes(2));

            _oauthFacade = new OAuth2AuthenticationFacade(
                _provider,
                clientId,
                callbackUrl,
                new OAuth2AuthenticationAdapter(),
                _securityStrategy);
        }

        /// <summary>
        /// Authenticates a resource owner using the OAuth2 token workflow
        /// </summary>
        /// <returns></returns>
        public virtual Task<OAuth2Credentials> GetCredentialsAsync()
        {
            var handler = new OAuth2FragmentCallbackHandler(
                _securityStrategy,
                OAuth2ParameterEnum.State.EnumToString());

            var authenticationUI = _uiFactory
                .GetAuthorizer<TResourceProvider, OAuth2Credentials>(
                    _browserType,
                    handler,
                    _callbackUri);

            var template = new OAuth2TokenAuthenticationTemplate(
                    authenticationUI,
                    _oauthFacade);

            return template.GetAccessTokenCredentials(
                Guid.NewGuid().ToString());
        }

        /// <summary>
        /// Authenticates a resource owner using the OAuth2 code workflow
        /// </summary>
        /// <param name="clientSecret">The client secret for the application</param>
        /// <returns></returns>
        public virtual Task<OAuth2Credentials> GetCredentialsAsync(
            string clientSecret)
        {
            var handler = new OAuth2QueryCallbackHandler(
                _securityStrategy,
                OAuth2ParameterEnum.State.EnumToString());

            var authenticationUI = _uiFactory
                .GetAuthorizer<TResourceProvider, OAuth2Credentials>(
                    _browserType,
                    handler,
                    _callbackUri);

            var template = new OAuth2CodeAuthenticationTemplate(
                    authenticationUI,
                    _oauthFacade,
                    clientSecret);

            return template.GetAccessTokenCredentials(
                Guid.NewGuid().ToString());
        }

        /// <summary>
        /// Adds scope to be requested with OAuth2 authentication
        /// </summary>
        /// <typeparam name="TRequest">The request type scope is needed for</typeparam>
        /// <returns>The current instance</returns>
        public OAuth2AppBase<TResourceProvider> AddScope<TRequest>()
            where TRequest : OAuthRequest, new()
        {
            _provider.AddRequestScope<TRequest>();

            return this;
        }
    }
}
