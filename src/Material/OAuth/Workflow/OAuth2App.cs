using System;
using System.Threading.Tasks;
using Foundations.Extensions;
using Foundations.HttpClient.Cryptography;
using Foundations.HttpClient.Cryptography.Algorithms;
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
using OAuth2ResponseType = Foundations.HttpClient.Enums.OAuth2ResponseType;

namespace Material.OAuth.Workflow
{
    public class OAuth2App<TResourceProvider>
        where TResourceProvider : OAuth2ResourceProvider, new()
    {
        protected readonly string _clientId;
        private readonly Uri _callbackUri;
        private readonly IOAuthAuthorizerUIFactory _uiFactory;
        private readonly IOAuthCallbackHandler<OAuth2Credentials> _callbackHandler;
        private readonly IOAuthAuthorizationAdapter _authAdapter;
        protected readonly IOAuthSecurityStrategy _securityStrategy;
        protected readonly ICryptoStringGenerator _requestIdGenerator;
        protected readonly TResourceProvider _provider;
        protected readonly AuthorizationInterface _browserType;

        public OAuth2App(
            string clientId,
            Uri callbackUri,
            IOAuthAuthorizerUIFactory uiFactory,
            IOAuthSecurityStrategy securityStrategy,
            IOAuthCallbackHandler<OAuth2Credentials> callbackHandler,
            IOAuthAuthorizationAdapter authAdapter,
            ICryptoStringGenerator requestIdGenerator,
            IAuthorizationUISelector authorizerSelector,
            TResourceProvider provider,
            AuthorizationInterface browserType)
        {
            if (uiFactory == null) throw new ArgumentNullException(nameof(uiFactory), StringResources.InitializationError);
            if (authorizerSelector == null) throw new ArgumentNullException(nameof(authorizerSelector), StringResources.InitializationError);

            if (clientId == null) throw new ArgumentNullException(nameof(clientId));
            if (callbackUri == null) throw new ArgumentNullException(nameof(callbackUri));
            if (securityStrategy == null) throw new ArgumentNullException(nameof(securityStrategy));
            if (callbackHandler == null) throw new ArgumentNullException(nameof(callbackHandler));
            if (authAdapter == null) throw new ArgumentNullException(nameof(authAdapter));
            if (requestIdGenerator == null) throw new ArgumentNullException(nameof(requestIdGenerator));
            if (provider == null) throw new ArgumentNullException(nameof(provider));

            _clientId = clientId;
            _callbackUri = callbackUri;
            _provider = provider;
            _uiFactory = uiFactory;
            _securityStrategy = securityStrategy;
            _callbackHandler = callbackHandler;
            _authAdapter = authAdapter;
            _requestIdGenerator = requestIdGenerator;

            _browserType = authorizerSelector.GetOptimalOAuth2Interface(
                provider,
                browserType,
                callbackUri);
        }

        public OAuth2App(
            string clientId,
            string callbackUri,
            IOAuthSecurityStrategy securityStrategy) : 
                this(
                    clientId, 
                    new Uri(callbackUri), 
                    QuantfabricConfiguration.WebAuthorizationUIFactory,
                    securityStrategy,
                    new OAuth2CallbackHandler(
                        securityStrategy, 
                        OAuth2Parameter.State.EnumToString()),
                    new OAuthAuthorizationAdapter(),
                    new CryptoStringGenerator(), 
                    QuantfabricConfiguration.WebAuthenticationUISelector,
                    new TResourceProvider(), 
                    AuthorizationInterface.NotSpecified)
        { }

        /// <summary>
        /// Authorizes a resource owner using the OAuth2 workflow
        /// </summary>
        /// <param name="clientId">The application's clientId</param>
        /// <param name="callbackUri">The application's registered callback url</param>
        public OAuth2App(
            string clientId,
            string callbackUri) :
                this(
                    clientId, 
                    callbackUri,
                    new OAuthSecurityStrategy(
                        new InMemoryCryptographicParameterRepository(),
                        QuantfabricConfiguration.SecurityParameterTimeout))
        { }

        /// <summary>
        /// Authorize a resource owner using the OAuth2 code workflow
        /// </summary>
        /// <param name="clientSecret">The client secret for the application</param>
        /// <returns>Valid OAuth2 credentials</returns>
        public Task<OAuth2Credentials> GetCredentialsAsync(string clientSecret)
        {
            return GetCredentialsWithRequestIdAsync(
                clientSecret,
                _requestIdGenerator.CreateRandomString());
        }

        /// <summary>
        /// Authorize a resource owner using a mobile workflow
        /// </summary>
        /// <returns>Valid OAuth2 credentials</returns>
        public Task<OAuth2Credentials> GetCredentialsAsync()
        {
            return GetCredentialsWithRequestIdAsync(
                _requestIdGenerator.CreateRandomString());
        }

        /// <summary>
        /// Authorize a resource owner using the OAuth2 code workflow
        /// </summary>
        /// <param name="clientSecret">The client secret for the application</param>
        /// <param name="requestId">The unique ID of the request NOTE THIS MUST AT LEAST BE UNIQUE PER USER</param>
        /// <returns>Valid OAuth2 credentials</returns>
        public Task<OAuth2Credentials> GetCredentialsWithRequestIdAsync(
            string clientSecret,
            string requestId)
        {
            if (clientSecret == null) throw new ArgumentNullException(nameof(clientSecret));
            if (requestId == null) throw new ArgumentNullException(nameof(requestId));

            _provider.SetClientProperties(
                _clientId, 
                clientSecret);

            return GetCredentialsAsync(
                OAuth2FlowType.AccessCode,
                OAuth2ResponseType.Code,
                CreateUriFacade(),
                CreateCodeFacade(clientSecret),
                requestId);
        }

        /// <summary>
        /// Authorize a resource owner using a mobile workflow
        /// </summary>
        /// <param name="requestId">The unique ID of the request NOTE THIS MUST AT LEAST BE UNIQUE PER USER</param>
        /// <returns>Valid OAuth2 credentials</returns>
        public Task<OAuth2Credentials> GetCredentialsWithRequestIdAsync(
            string requestId)
        {
            if (requestId == null) throw new ArgumentNullException(nameof(requestId));

            if ((_browserType == AuthorizationInterface.Dedicated || 
                 _browserType == AuthorizationInterface.SecureEmbedded) &&
                 _provider.SupportsPkce)
            {
                return GetCredentialsAsync(
                        OAuth2FlowType.AccessCode,
                        OAuth2ResponseType.Code,
                        CreateUriFacade(
                            new OAuth2Sha256PkceSecurityParameterBundle(
                                DigestSigningAlgorithm.Sha256Algorithm())),
                        CreateCodeFacade(
                                new OAuth2PkceVerifierSecurityParameterBundle()),
                        requestId);
            }
            else
            {
                return GetCredentialsAsync(
                    OAuth2FlowType.Implicit,
                    OAuth2ResponseType.Token,
                    CreateUriFacade(),
                    CreateTokenFacade(),
                    requestId);
            }
        }

        protected OAuth2ImplicitFacade CreateTokenFacade()
        {
            return new OAuth2ImplicitFacade(
                _clientId,
                _provider);
        }

        protected OAuth2AccessCodeFacade CreateCodeFacade(
            params ISecurityParameterBundle[] securityParameters)
        {
            return new OAuth2AccessCodeFacade(
                    _provider,
                    _clientId,
                    _callbackUri,
                    _authAdapter,
                    _securityStrategy)
                .AddSecurityParameters(securityParameters);
        }

        protected OAuth2AccessCodeFacade CreateCodeFacade(
            string clientSecret,
            params ISecurityParameterBundle[] securityParameters)
        {
            return new OAuth2AccessCodeFacade(
                    _provider,
                    _clientId,
                    clientSecret,
                    _callbackUri,
                    _authAdapter,
                    _securityStrategy)
                .AddSecurityParameters(securityParameters);
        }

        protected OAuth2AuthorizationUriFacade CreateUriFacade(
            params ISecurityParameterBundle[] securityParameters)
        {
            return new OAuth2AuthorizationUriFacade(
                    _provider,
                    _clientId,
                    _callbackUri,
                    _authAdapter,
                    _securityStrategy)
                .AddSecurityParameters(securityParameters);
        }

        protected Task<OAuth2Credentials> GetCredentialsAsync(
            OAuth2FlowType flowType,
            OAuth2ResponseType responseType,
            IOAuthAuthorizationUriFacade uriFacade,
            IOAuthAccessTokenFacade<OAuth2Credentials> tokenFacade,
            string requestId)
        {
            return GetCredentialsAsync(
                flowType, 
                responseType, 
                uriFacade, 
                tokenFacade, 
                requestId, 
                true);
        }

        protected async Task<OAuth2Credentials> GetCredentialsAsync(
            OAuth2FlowType flowType,
            OAuth2ResponseType responseType,
            IOAuthAuthorizationUriFacade uriFacade,
            IOAuthAccessTokenFacade<OAuth2Credentials> tokenFacade,
            string requestId,
            bool clearSecurityParameters)
        {
            if (uriFacade == null) throw new ArgumentNullException(nameof(uriFacade));
            if (tokenFacade == null) throw new ArgumentNullException(nameof(tokenFacade));

            _securityStrategy.ClearSecureParameters(requestId);

            _provider.SetFlow(flowType);
            _provider.SetResponse(responseType);

            var authenticationUi = _uiFactory
                .GetAuthorizer<TResourceProvider, OAuth2Credentials>(
                    _browserType,
                    _callbackHandler,
                    _callbackUri);

            var template = new OAuthAuthorizationTemplate<OAuth2Credentials>(
                    authenticationUi,
                    uriFacade,
                    tokenFacade,
                    _securityStrategy);

            var result = await template
                .GetAccessTokenCredentials(requestId)
                .ConfigureAwait(false);

            if (clearSecurityParameters)
            {
                _securityStrategy.ClearSecureParameters(requestId);
            }

            return result;
        }

        /// <summary>
        /// Adds scope to be requested with OAuth2 authorization
        /// </summary>
        /// <typeparam name="TRequest">The request type scope is needed for</typeparam>
        /// <returns>The current instance</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        public OAuth2App<TResourceProvider> AddScope<TRequest>()
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
        public OAuth2App<TResourceProvider> AddScope(string scope)
        {
            _provider.AddRequestScope(scope);

            return this;
        }
    }
}
