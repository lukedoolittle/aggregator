using System;
using System.Threading.Tasks;
using Foundations.Extensions;
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
    public class OAuth2AppBase<TResourceProvider>
        where TResourceProvider : OAuth2ResourceProvider
    {
        private readonly string _clientId;
        private readonly Uri _callbackUri;
        private readonly IOAuthAuthorizerUIFactory _uiFactory;
        private readonly TResourceProvider _provider;
        private readonly AuthorizationInterface _browserType;

        public OAuth2AppBase(
            string clientId,
            Uri callbackUri,
            IOAuthAuthorizerUIFactory uiFactory,
            TResourceProvider provider,
            AuthorizationInterface browserType)
        {
            _clientId = clientId;
            _callbackUri = callbackUri;
            _browserType = browserType;
            _provider = provider;
            _uiFactory = uiFactory;
        }

        /// <summary>
        /// Authorize a resource owner using the OAuth2 code workflow
        /// </summary>
        /// <param name="clientSecret">The client secret for the application</param>
        /// <param name="requestId">The unique ID of the request</param>
        /// <returns>Valid OAuth2 credentials</returns>
        public Task<OAuth2Credentials> GetCredentialsAsync(
            string clientSecret,
            string requestId)
        {
            if (clientSecret == null) throw new ArgumentNullException(nameof(clientSecret));
            if (requestId == null) throw new ArgumentNullException(nameof(requestId));

            var securityStrategy = new OAuthSecurityStrategy(
                new InMemoryCryptographicParameterRepository(),
                TimeSpan.FromMinutes(
                    OAuthConfiguration.SecurityParameterTimeoutInMinutes));

            var callbackHandler = new OAuth2CallbackHandler(
                securityStrategy,
                OAuth2Parameter.State.EnumToString());

            _provider.SetClientProperties(
                _clientId, 
                clientSecret);

            return GetCredentialsAsync(
                OAuth2FlowType.AccessCode,
                OAuth2ResponseType.Code,
                callbackHandler,
                CreateUriFacade(
                    securityStrategy),
                CreateTokenFacade(
                    clientSecret, 
                    securityStrategy),
                securityStrategy,
                null,
                requestId);
        }

        /// <summary>
        /// Authorize a resource owner using a mobile workflow
        /// </summary>
        /// <returns>Valid OAuth2 credentials</returns>
        public Task<OAuth2Credentials> GetCredentialsAsync(
            string requestId)
        {
            if (requestId == null) throw new ArgumentNullException(nameof(requestId));

            var securityStrategy = new OAuthSecurityStrategy(
                new InMemoryCryptographicParameterRepository(),
                TimeSpan.FromMinutes(
                    OAuthConfiguration.SecurityParameterTimeoutInMinutes));

            var callbackHandler = new OAuth2CallbackHandler(
                securityStrategy,
                OAuth2Parameter.State.EnumToString());

            if ((_browserType == AuthorizationInterface.Dedicated || 
                 _browserType == AuthorizationInterface.SecureEmbedded) &&
                 _provider.SupportsPkce)
            {
                return GetCredentialsAsync(
                        OAuth2FlowType.AccessCode,
                        OAuth2ResponseType.Code,
                        callbackHandler,
                        CreateUriFacade(
                            securityStrategy,
                            new OAuth2Sha256PkceSecurityParameterBundle(
                                DigestSigningAlgorithm.Sha256Algorithm())),
                        CreateTokenFacade(securityStrategy)
                            .AddSecurityParameters(
                                new OAuth2PkceVerifierSecurityParameterBundle()),
                        securityStrategy,
                        null,
                        requestId);
            }
            else
            {
                return GetCredentialsAsync(
                    OAuth2FlowType.Implicit,
                    OAuth2ResponseType.Token,
                    callbackHandler,
                    CreateUriFacade(
                        securityStrategy),
                    new OAuth2ImplicitFacade(
                        _clientId,
                        _provider),
                    securityStrategy,
                    null,
                    requestId);
            }
        }

        public async Task<JsonWebToken> GetIdTokenAsync(
            IJsonWebTokenAuthenticationValidator validator,
            string requestId)
        {
            if (requestId == null) throw new ArgumentNullException(nameof(requestId));

            var securityStrategy = new OAuthSecurityStrategy(
                new InMemoryCryptographicParameterRepository(),
                TimeSpan.FromMinutes(
                    OAuthConfiguration.SecurityParameterTimeoutInMinutes));

            var callbackHandler = new OAuth2CallbackHandler(
                securityStrategy,
                OAuth2Parameter.State.EnumToString());

            if ((_browserType == AuthorizationInterface.Dedicated ||
                 _browserType == AuthorizationInterface.SecureEmbedded) &&
                 _provider.SupportsPkce)
            {
                var credentials = await GetCredentialsAsync(
                    OAuth2FlowType.AccessCode,
                    OAuth2ResponseType.Code,
                    callbackHandler,
                    CreateUriFacade(
                        securityStrategy,
                        new OAuth2NonceSecurityParameterBundle(),
                        new OAuth2Sha256PkceSecurityParameterBundle(
                            DigestSigningAlgorithm.Sha256Algorithm())),
                    CreateTokenFacade(securityStrategy)
                        .AddSecurityParameters(
                            new OAuth2PkceVerifierSecurityParameterBundle()),
                        securityStrategy,
                        validator,
                        requestId)
                    .ConfigureAwait(false);

                return credentials?.IdToken;
            }
            else
            {
                var credentials = await GetCredentialsAsync(
                        OAuth2FlowType.Implicit,
                        OAuth2ResponseType.IdTokenToken,
                        callbackHandler,
                        CreateUriFacade(
                            securityStrategy,
                            new OAuth2NonceSecurityParameterBundle()),
                        new OAuth2ImplicitFacade(
                            _clientId,
                            _provider),
                        securityStrategy,
                        validator,
                        requestId)
                    .ConfigureAwait(false);

                return credentials?.IdToken;
            }
        }

        public async Task<JsonWebToken> GetIdTokenAsync(
            string clientSecret,
            IJsonWebTokenAuthenticationValidator validator,
            string requestId)
        {
            if (clientSecret == null) throw new ArgumentNullException(nameof(clientSecret));
            if (requestId == null) throw new ArgumentNullException(nameof(requestId));

            var securityStrategy = new OAuthSecurityStrategy(
                new InMemoryCryptographicParameterRepository(),
                TimeSpan.FromMinutes(
                    OAuthConfiguration.SecurityParameterTimeoutInMinutes));

            var callbackHandler = new OAuth2CallbackHandler(
                securityStrategy,
                OAuth2Parameter.State.EnumToString());

            var credentials = await GetCredentialsAsync(
                    OAuth2FlowType.AccessCode,
                    OAuth2ResponseType.Code,
                    callbackHandler,
                    CreateUriFacade(
                        securityStrategy,
                        new OAuth2NonceSecurityParameterBundle()),
                    CreateTokenFacade(
                        clientSecret,
                        securityStrategy),
                    securityStrategy,
                    validator,
                    requestId)
                .ConfigureAwait(false);

            return credentials?.IdToken;
        }

        private OAuth2AccessCodeFacade CreateTokenFacade(
            IOAuthSecurityStrategy securityStrategy)
        {
            var adapter = new OAuthAuthorizationAdapter();

            return new OAuth2AccessCodeFacade(
                _provider,
                _clientId,
                _callbackUri,
                adapter,
                securityStrategy);
        }

        private OAuth2AccessCodeFacade CreateTokenFacade(
            string clientSecret, 
            IOAuthSecurityStrategy securityStrategy)
        {
            var adapter = new OAuthAuthorizationAdapter();

            return new OAuth2AccessCodeFacade(
                _provider,
                _clientId,
                clientSecret,
                _callbackUri,
                adapter,
                securityStrategy);
        }

        private OAuth2AuthorizationUriFacade CreateUriFacade(
            IOAuthSecurityStrategy securityStrategy,
            params ISecurityParameterBundle[] securityParameters)
        {
            var adapter = new OAuthAuthorizationAdapter();

            return new OAuth2AuthorizationUriFacade(
                    _provider,
                    _clientId,
                    _callbackUri,
                    adapter,
                    securityStrategy)
                .AddSecurityParameters(securityParameters);
        }

        private async Task<OAuth2Credentials> GetCredentialsAsync(
            OAuth2FlowType flowType,
            OAuth2ResponseType responseType,
            IOAuthCallbackHandler<OAuth2Credentials> callbackHandler,
            IOAuthAuthorizationUriFacade uriFacade,
            IOAuthAccessTokenFacade<OAuth2Credentials> tokenFacade,
            IOAuthSecurityStrategy securityStrategy,
            IJsonWebTokenAuthenticationValidator validator,
            string requestId)
        {
            if (callbackHandler == null) throw new ArgumentNullException(nameof(callbackHandler));
            if (uriFacade == null) throw new ArgumentNullException(nameof(uriFacade));
            if (tokenFacade == null) throw new ArgumentNullException(nameof(tokenFacade));

            securityStrategy.ClearSecureParameters(requestId);

            _provider.SetFlow(flowType);
            _provider.SetResponse(responseType);

            var authenticationUi = _uiFactory
                .GetAuthorizer<TResourceProvider, OAuth2Credentials>(
                    _browserType,
                    callbackHandler,
                    _callbackUri);

            var template = new OAuthAuthorizationTemplate<OAuth2Credentials>(
                    authenticationUi,
                    uriFacade,
                    tokenFacade,
                    securityStrategy);

            var result = await template
                .GetAccessTokenCredentials(requestId)
                .ConfigureAwait(false);

            validator?.IsTokenValid(result.IdToken);

            securityStrategy.ClearSecureParameters(requestId);

            return result;
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
