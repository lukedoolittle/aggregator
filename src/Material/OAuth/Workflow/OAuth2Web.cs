﻿using System;
using System.Threading.Tasks;
using Foundations.Extensions;
using Foundations.HttpClient.Cryptography;
using Foundations.HttpClient.Enums;
using Material.Contracts;
using Material.Infrastructure;
using Material.Infrastructure.Credentials;
using Material.OAuth.Authorization;
using Material.OAuth.Callback;
using Material.OAuth.Facade;
using Material.OAuth.Security;

namespace Material.OAuth.Workflow
{
    public class OAuth2Web<TResourceProvider>
        where TResourceProvider : OAuth2ResourceProvider, new()
    {
        private readonly string _clientId;
        private readonly string _clientSecret;
        private readonly IOAuthAuthorizationUriFacade _uriFacade;
        private readonly IOAuthAccessTokenFacade<OAuth2Credentials> _accessTokenFacade;
        private readonly IOAuthCallbackHandler<OAuth2Credentials> _callbackHandler;
        private readonly TResourceProvider _resourceProvider;
        private readonly IOAuthSecurityStrategy _securityStrategy;
        private readonly ICryptoStringGenerator _idGenerator;

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
            _clientId = clientId;
            _clientSecret = clientSecret;
            _resourceProvider = resourceProvider;
            _callbackHandler = callbackHandler;
            _uriFacade = uriFacade;
            _accessTokenFacade = accessTokenFacade;
            _securityStrategy = securityStrategy;
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
        public Task<Uri> GetAuthorizationUriAsync()
        {
            _resourceProvider.SetFlow(OAuth2FlowType.AccessCode);

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
            _resourceProvider.SetClientProperties(
                _clientId,
                _clientSecret);

            var intermediateResult = _callbackHandler
                .ParseAndValidateCallback(
                    responseUri);

            var token = await _accessTokenFacade
                .GetAccessTokenAsync(
                    intermediateResult.Credentials, 
                    intermediateResult.RequestId)
                .ConfigureAwait(false);

            if (!shouldSkipClearParameters)
            {
                _securityStrategy.ClearSecureParameters(
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

        public string GetRequestIdFromResponse(Uri responseUri)
        {
            if (responseUri == null) throw new ArgumentNullException(nameof(responseUri));

            var intermediateResult = _callbackHandler
                .ParseAndValidateCallback(
                    responseUri);

            return intermediateResult.RequestId;
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
            _resourceProvider.AddRequestScope<TRequest>();

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
            _resourceProvider.AddRequestScope(scope);

            return this;
        }
    }
}
