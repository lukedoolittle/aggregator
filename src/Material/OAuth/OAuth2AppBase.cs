using System;
using System.Threading.Tasks;
using Foundations.HttpClient.Enums;
using Material.Contracts;
using Material.Enums;
using Material.Infrastructure;
using Material.Infrastructure.Credentials;
using Material.OAuth.Authorization;
using Material.OAuth.Facade;
using Material.OAuth.Template;

namespace Material.OAuth
{
    public class OAuth2AppBase<TResourceProvider>
        where TResourceProvider : OAuth2ResourceProvider
    {
        private readonly Uri _callbackUri;
        private readonly IOAuthAuthorizerUIFactory _uiFactory;
        private readonly TResourceProvider _provider;
        private readonly AuthorizationInterface _browserType;
        private readonly string _clientId;
        private readonly IOAuthSecurityStrategy _securityStrategy;

        public OAuth2AppBase(
            string clientId,
            Uri callbackUri,
            IOAuthAuthorizerUIFactory uiFactory,
            IOAuthSecurityStrategy securityStrategy,
            TResourceProvider provider,
            AuthorizationInterface browserType)
        {
            _callbackUri = callbackUri;
            _browserType = browserType;
            _provider = provider;
            _uiFactory = uiFactory;
            _clientId = clientId;
            _securityStrategy = securityStrategy;
        }

        /// <summary>
        /// Authenticates a resource owner using the OAuth2 token workflow
        /// </summary>
        /// <param name="flowType">OAuth2 workflow type</param>
        /// <param name="responseType">OAuth2 response type to request</param>
        /// <param name="callbackHandler">Handles the callback request</param>
        /// <returns></returns>
        public virtual Task<OAuth2Credentials> GetCredentialsAsync(
            OAuth2FlowType flowType,
            OAuth2ResponseType responseType,
            IOAuthCallbackHandler<OAuth2Credentials> callbackHandler)
        {
            var facade = new OAuth2TokenAuthorizationFacade(
                _provider,
                _clientId,
                _callbackUri,
                new OAuth2AuthorizationAdapter(),
                _securityStrategy);

            return GetCredentialsAsync(
                null,
                flowType,
                responseType,
                callbackHandler,
                facade);
        }

        /// <summary>
        /// Authenticates a resource owner using the OAuth2 code workflow
        /// </summary>
        /// <param name="clientSecret">The client secret for the application</param>
        /// <param name="flowType">OAuth2 workflow type</param>
        /// <param name="responseType">OAuth2 response type to request</param>
        /// <param name="callbackHandler">Handles the callback request</param>
        /// <returns></returns>
        public virtual Task<OAuth2Credentials> GetCredentialsAsync(
            string clientSecret,
            OAuth2FlowType flowType,
            OAuth2ResponseType responseType,
            IOAuthCallbackHandler<OAuth2Credentials> callbackHandler)
        {
            var facade = new OAuth2CodeAuthorizationFacade(
                _provider,
                _clientId,
                _callbackUri,
                new OAuth2AuthorizationAdapter(),
                _securityStrategy);

            return GetCredentialsAsync(
                clientSecret, 
                flowType, 
                responseType,
                callbackHandler, 
                facade);
        }

        private Task<OAuth2Credentials> GetCredentialsAsync(
            string clientSecret,
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

            var template = new OAuth2AuthorizationTemplate(
                    authenticationUI,
                    facade,
                    clientSecret);

            return template.GetAccessTokenCredentials(
                Guid.NewGuid().ToString());
        }

        /// <summary>
        /// Adds scope to be requested with OAuth2 authentication
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
    }
}
