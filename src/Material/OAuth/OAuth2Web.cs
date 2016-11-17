﻿using System;
using System.Threading.Tasks;
using Foundations.Extensions;
using Foundations.HttpClient.Enums;
using Material.Contracts;
using Material.Infrastructure;
using Material.Infrastructure.Credentials;
using Material.OAuth.Authorization;
using Material.OAuth.Callback;
using Material.OAuth.Facade;
using Material.OAuth.Security;

namespace Material.OAuth
{
    public class OAuth2Web<TResourceProvider>
        where TResourceProvider : OAuth2ResourceProvider, new()
    {
        private readonly string _clientSecret;
        private readonly IOAuthFacade<OAuth2Credentials> _authFacade;
        private readonly IOAuthCallbackHandler<OAuth2Credentials> _callbackHandler;
        private readonly TResourceProvider _resourceProvider;

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
            TResourceProvider resourceProvider)
        {
            _clientSecret = clientSecret;
            _resourceProvider = resourceProvider;

            _callbackHandler = new OAuth2CallbackHandler(
                strategy,
                OAuth2Parameter.State.EnumToString());

            _authFacade = new OAuth2CodeAuthorizationFacade(
                _resourceProvider,
                clientId,
                new Uri(callbackUrl),
                new OAuth2AuthorizationAdapter(),
                strategy);

            //NOTE: you could create a token workflow version of this
            //but why???
            //
            //var handler = new OAuth2QueryCallbackHandler(
            //  _strategy,
            //  OAuth2Parameter.State.EnumToString(),
            //  userId);
            //
            //_authFacade = new OAuth2TokenAuthorizationFacade(
            //    _resourceProvider,
            //    clientId,
            //    new Uri(callbackUrl),
            //    new OAuth2AuthorizationAdapter(),
            //    strategy);
        }

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
                        TimeSpan.FromMinutes(2)), 
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
                    new OAuthSecurityStrategy(
                        new InMemoryCryptographicParameterRepository(),
                        TimeSpan.FromMinutes(2)),
                    new TResourceProvider())
        {}



        /// <summary>
        /// Gets the authorization uri for the Resource Owner to enter his/her credentials
        /// </summary>
        /// <param name="userId">Resource owner's Id</param>
        /// <returns>Authorization uri</returns>
        public Task<Uri> GetAuthorizationUriAsync(string userId)
        {
            return _authFacade.GetAuthorizationUriAsync(userId);
        }

        /// <summary>
        /// Exchanges callback uri for access token credentials
        /// </summary>
        /// <param name="userId">Resource owner's Id</param>
        /// <param name="responseUri">The received callback uri</param>
        /// <returns>Access token credentials</returns>
        public Task<OAuth2Credentials> GetAccessTokenAsync(
            Uri responseUri,
            string userId)
        {
            var result = _callbackHandler
                        .ParseAndValidateCallback(
                            responseUri, 
                            userId);

            return _authFacade.GetAccessTokenAsync(
                result, 
                _clientSecret);
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
