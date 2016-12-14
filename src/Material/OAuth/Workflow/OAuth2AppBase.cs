using System;
using System.Threading.Tasks;
using Foundations.Extensions;
using Foundations.HttpClient.Enums;
using Material.Contracts;
using Material.Enums;
using Material.Infrastructure;
using Material.Infrastructure.Credentials;
using Material.OAuth.Authorization;
using Material.OAuth.Callback;
using Material.OAuth.Facade;
using Material.OAuth.Security;
using Material.OAuth.Template;

namespace Material.OAuth.Workflow
{
    public class OAuth2AppBase<TResourceProvider>
        where TResourceProvider : OAuth2ResourceProvider
    {
        private readonly string _clientId;
        private readonly Uri _callbackUri;
        private readonly IOAuthAuthorizerUIFactory _uiFactory;
        private readonly TResourceProvider _provider;
        private readonly AuthorizationInterface _browserType;
        private readonly string _userId;
        private readonly IOAuthSecurityStrategy _securityStrategy;
        private readonly OAuth2CallbackHandler _callbackHandler;

        public OAuth2AppBase(
            string clientId,
            Uri callbackUri,
            IOAuthAuthorizerUIFactory uiFactory,
            TResourceProvider provider,
            AuthorizationInterface browserType,
            string userId)
        {
            _clientId = clientId;
            _callbackUri = callbackUri;
            _browserType = browserType;
            _provider = provider;
            _uiFactory = uiFactory;
            _userId = userId;

            _securityStrategy = new OAuthSecurityStrategy(
                new InMemoryCryptographicParameterRepository(),
                TimeSpan.FromMinutes(
                    OAuthConfiguration.SecurityParameterTimeoutInMinutes));

            _callbackHandler = new OAuth2CallbackHandler(
                _securityStrategy,
                OAuth2Parameter.State.EnumToString());
        }

        /// <summary>
        /// Authorize a resource owner using the OAuth2 code workflow
        /// </summary>
        /// <param name="clientSecret">The client secret for the application</param>
        /// <returns>Valid OAuth2 credentials</returns>
        public virtual Task<OAuth2Credentials> GetCredentialsAsync(
            string clientSecret)
        {
            var facade = new OAuth2CodeAuthorizationFacade(
                _provider,
                _clientId,
                clientSecret,
                _callbackUri,
                new OAuth2AuthorizationAdapter(),
                _securityStrategy);

            return GetCredentialsAsync(
                OAuth2FlowType.AccessCode,
                OAuth2ResponseType.Code,
                _callbackHandler,
                facade);
        }

        /// <summary>
        /// Authorize a resource owner using a mobile workflow
        /// </summary>
        /// <returns>Valid OAuth2 credentials</returns>
        public virtual Task<OAuth2Credentials> GetCredentialsAsync()
        {
            if ((_browserType == AuthorizationInterface.Dedicated || 
                _browserType == AuthorizationInterface.SecureEmbedded) &&
                _provider.SupportsPkce)
            {
                var codeFacade = new OAuth2CodeWithPkcsAuthorizationFacade(
                    _provider,
                    _clientId,
                    _callbackUri,
                    new OAuth2AuthorizationAdapter(),
                    _securityStrategy);

                return GetCredentialsAsync(
                        OAuth2FlowType.AccessCode,
                        OAuth2ResponseType.Code,
                        _callbackHandler,
                        codeFacade);
            }
            else
            {
                var tokenFacade = new OAuth2TokenAuthorizationFacade(
                    _provider,
                    _clientId,
                    _callbackUri,
                    new OAuth2AuthorizationAdapter(),
                    _securityStrategy);

                return GetCredentialsAsync(
                    OAuth2FlowType.Implicit,
                    OAuth2ResponseType.Token,
                    _callbackHandler,
                    tokenFacade);
            }
        }

        private Task<OAuth2Credentials> GetCredentialsAsync(
            OAuth2FlowType flowType,
            OAuth2ResponseType responseType,
            IOAuthCallbackHandler<OAuth2Credentials> callbackHandler,
            IOAuthFacade<OAuth2Credentials> facade)
        {
            if (facade == null) throw new ArgumentNullException(nameof(facade));

            _provider.SetFlow(flowType);
            _provider.SetResponse(responseType);

            var authenticationUI = _uiFactory
                .GetAuthorizer<TResourceProvider, OAuth2Credentials>(
                    _browserType,
                    callbackHandler,
                    _callbackUri);

            var template = new OAuthAuthorizationTemplate<OAuth2Credentials>(
                    authenticationUI,
                    facade);

            return template.GetAccessTokenCredentials(
                _userId);
        }

        /// <summary>
        /// Adds scope to be requested with OAuth2 authorization
        /// </summary>
        /// <typeparam name="TRequest">The request type scope is needed for</typeparam>
        /// <returns>The current instance</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        public OAuth2AppBase<TResourceProvider> AddScope<TRequest>()
            where TRequest : OAuthRequest, new()
        {
            _provider.AddRequestScope<TRequest>();

            return this;
        }

        /// <summary>
        /// Adds scope to be requested with OAuth2 authorization
        /// </summary>
        /// <param name="scope">The scope to request</param>
        /// <returns>The current instance</returns>
        public OAuth2AppBase<TResourceProvider> AddScope(string scope)
        {
            _provider.AddRequestScope(scope);

            return this;
        }
    }
}
