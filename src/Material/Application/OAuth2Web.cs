using System;
using Material.Domain.Credentials;
using System.Threading.Tasks;
using Material.Authorization;
using Material.Contracts;
using Material.Domain.Core;
using Material.Framework.Enums;
using Material.Framework.Extensions;
using Material.HttpClient.Cryptography;
using Material.Workflow.Callback;
using Material.Workflow.Facade;
using Material.Workflow.Security;

namespace Material.Application
{
    public class OAuth2Web<TResourceProvider>
        where TResourceProvider : OAuth2ResourceProvider, new()
    {
        private readonly string _clientSecret;
        private readonly IOAuthAuthorizationUriFacade _uriFacade;
        private readonly IOAuthAccessTokenFacade<OAuth2Credentials> _accessTokenFacade;
        private readonly ICryptoStringGenerator _idGenerator;

        protected IOAuthCallbackHandler<OAuth2Credentials> CallbackHandler { get; }
        protected IOAuthSecurityStrategy SecurityStrategy { get; }
        protected string ClientId { get; }
        protected TResourceProvider ResourceProvider { get; }

        /// <summary>
        /// Authorize a resource owner using the OAuth2 workflow with default security strategy
        /// </summary>
        /// <param name="clientId">The application's client Id</param>
        /// <param name="clientSecret">The application's client secret</param>
        /// <param name="resourceProvider">Endpoint information for the resource provider</param>
        /// <param name="callbackHandler">Handles the authorization uris callback response</param>
        /// <param name="uriFacade">Creates the authorization uri</param>
        /// <param name="accessTokenFacade">Exchanges code for an access token</param>
        /// <param name="securityStrategy">Security manager for OAuth calls</param>
        /// <param name="idGenerator">Generator to create request Ids</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1054:UriParametersShouldNotBeStrings", MessageId = "2#")]
        public OAuth2Web(
            string clientId,
            string clientSecret,
            TResourceProvider resourceProvider,
            IOAuthCallbackHandler<OAuth2Credentials> callbackHandler,
            IOAuthAuthorizationUriFacade uriFacade,
            IOAuthAccessTokenFacade<OAuth2Credentials> accessTokenFacade,
            IOAuthSecurityStrategy securityStrategy,
            ICryptoStringGenerator idGenerator)
        {
            ClientId = clientId;
            _clientSecret = clientSecret;
            ResourceProvider = resourceProvider;
            CallbackHandler = callbackHandler;
            _uriFacade = uriFacade;
            _accessTokenFacade = accessTokenFacade;
            SecurityStrategy = securityStrategy;
            _idGenerator = idGenerator;
        }

        /// <summary>
        /// Authorize a resource owner using the OAuth2 workflow with default security strategy
        /// </summary>
        /// <param name="clientId">The application's client Id</param>
        /// <param name="clientSecret">The application's client secret</param>
        /// <param name="callbackUrl">The application's registered callback url</param>
        /// <param name="strategy">The security strategy to use for "state" handling</param>
        /// <param name="resourceProvider">Endpoint information for the resource provider</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1054:UriParametersShouldNotBeStrings", MessageId = "2#")]
        public OAuth2Web(
            string clientId,
            string clientSecret,
            string callbackUrl,
            IOAuthSecurityStrategy strategy,
            TResourceProvider resourceProvider) : 
                this(
                    clientId,
                    clientSecret, 
                    resourceProvider, 
                    new OAuth2CallbackHandler(
                        strategy,
                        OAuth2Parameter.State.EnumToString()), 
                    new OAuth2AuthorizationUriFacade(
                            resourceProvider, 
                            clientId, 
                            new Uri(callbackUrl), 
                            new OAuthAuthorizationAdapter(), 
                            strategy),
                    new OAuth2AccessCodeFacade(
                        resourceProvider, 
                        clientId, 
                        clientSecret,
                        new Uri(callbackUrl), 
                        new OAuthAuthorizationAdapter(), 
                        strategy),
                    strategy, 
                    new CryptoStringGenerator())
        { }

        /// <summary>
        /// Authorize a resource owner using the OAuth2 workflow with default security strategy
        /// </summary>
        /// <param name="clientId">The application's client Id</param>
        /// <param name="clientSecret">The application's client secret</param>
        /// <param name="callbackUri">The application's registered callback url</param>
        /// <param name="resourceProvider">Endpoint information for the resource provider</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1054:UriParametersShouldNotBeStrings", MessageId = "2#")]
        public OAuth2Web(
            string clientId,
            string clientSecret,
            string callbackUri,
            TResourceProvider resourceProvider) : 
                this(
                    clientId, 
                    clientSecret,
                    callbackUri,
                    new OAuthSecurityStrategy(
                        new InMemoryCryptographicParameterRepository(),
                        QuantfabricConfiguration.SecurityParameterTimeout), 
                    resourceProvider)
        {}

        /// <summary>
        /// Authorize a resource owner using the OAuth2 workflow with default security strategy
        /// </summary>
        /// <param name="clientId">The application's client Id</param>
        /// <param name="clientSecret">The application's client secret</param>
        /// <param name="callbackUrl">The application's registered callback url</param>
        /// <param name="strategy">The security strategy to use for "state" handling</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1054:UriParametersShouldNotBeStrings", MessageId = "2#")]
        public OAuth2Web(
            string clientId,
            string clientSecret,
            string callbackUrl,
            IOAuthSecurityStrategy strategy) :
                this(
                    clientId,
                    clientSecret, 
                    callbackUrl, 
                    strategy, 
                    new TResourceProvider())
        {}

        /// <summary>
        /// Authorize a resource owner using the OAuth2 workflow with default security strategy
        /// </summary>
        /// <param name="clientId">The application's client Id</param>
        /// <param name="clientSecret">The application's client secret</param>
        /// <param name="callbackUri">The application's registered callback url</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1054:UriParametersShouldNotBeStrings", MessageId = "2#")]
        public OAuth2Web(
            string clientId,
            string clientSecret,
            string callbackUri) :
                this(
                    clientId,
                    clientSecret,
                    callbackUri,
                    new TResourceProvider())
        {}

        /// <summary>
        /// Gets the authorization uri for the Resource Owner to enter his/her credentials
        /// </summary>
        /// <returns>Authorization uri</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")]
        public virtual Task<Uri> GetAuthorizationUriAsync()
        {
            ResourceProvider.SetFlow(OAuth2FlowType.AccessCode);

            return _uriFacade.GetAuthorizationUriAsync(
                _idGenerator.CreateRandomString());
        }

        /// <summary>
        /// Exchanges callback uri for access token credentials
        /// </summary>
        /// <param name="responseUri">The received callback uri</param>
        /// <param name="shouldSkipClearParameters"></param>
        /// <returns></returns>
        public async Task<OAuth2Credentials> GetAccessTokenAsync(
            Uri responseUri,
            bool shouldSkipClearParameters)
        {
            ResourceProvider.SetClientProperties(
                ClientId,
                _clientSecret);

            var intermediateResult = CallbackHandler
                .ParseAndValidateCallback(
                    responseUri);

            var token = await _accessTokenFacade
                .GetAccessTokenAsync(
                    intermediateResult.Credentials, 
                    intermediateResult.RequestId)
                .ConfigureAwait(false);

            if (!shouldSkipClearParameters)
            {
                SecurityStrategy.ClearSecureParameters(
                    intermediateResult.RequestId);
            }

            return token;
        }

        /// <summary>
        /// Exchanges callback uri for access token credentials
        /// </summary>
        /// <param name="responseUri">The received callback uri</param>
        /// <returns>Access token credentials</returns>
        public Task<OAuth2Credentials> GetAccessTokenAsync(
            Uri responseUri)
        {
            return GetAccessTokenAsync(responseUri, false);
        }

        /// <summary>
        /// Adds scope to be requested with OAuth2 Authorize
        /// </summary>
        /// <typeparam name="TRequest">The request type scope is needed for</typeparam>
        /// <returns>The current instance</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        public OAuth2Web<TResourceProvider> AddScope<TRequest>()
            where TRequest : OAuthRequest, new()
        {
            ResourceProvider.AddRequestScope<TRequest>();

            return this;
        }

        /// <summary>
        /// Adds scope to be requested with OAuth2 authorization
        /// </summary>
        /// <param name="scope">The scope to request</param>
        /// <returns>The current instance</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        public OAuth2Web<TResourceProvider> AddScope(string scope)
        {
            ResourceProvider.AddRequestScope(scope);

            return this;
        }
    }
}
