using System;
using System.Threading.Tasks;
using Foundations.Extensions;
using Foundations.HttpClient.Cryptography;
using Foundations.HttpClient.Enums;
using Material.Contracts;
using Material.Infrastructure;
using Material.Infrastructure.Credentials;
using Material.OAuth.Authentication;
using Material.OAuth.Authorization;
using Material.OAuth.Callback;
using Material.OAuth.Facade;
using Material.OAuth.Security;

namespace Material.OAuth.Workflow
{
    public class OpenIdWeb<TResourceProvider>
        where TResourceProvider : OpenIdResourceProvider, new()
    {
        private readonly OAuth2Web<TResourceProvider> _web;
        private readonly TResourceProvider _provider;
        private readonly string _clientId;
        private readonly IOAuthSecurityStrategy _securityStrategy;

        /// <summary>
        /// Authenticate a resource owner using the OpenId Connect workflow with default security strategy
        /// </summary>
        /// <param name="clientId">The application's client Id</param>
        /// <param name="clientSecret">The application's client secret</param>
        /// <param name="callbackUrl">The application's registered callback url</param>
        /// <param name="strategy">The security strategy to use for "state" handling</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1054:UriParametersShouldNotBeStrings", MessageId = "2#")]
        public OpenIdWeb(
            string clientId,
            string clientSecret,
            string callbackUrl,
            IOAuthSecurityStrategy strategy)
        {
            _clientId = clientId;
            _securityStrategy = strategy;

            _provider = new TResourceProvider();

            var callbackHandler = new OAuth2CallbackHandler(
                strategy,
                OAuth2Parameter.State.EnumToString());

            var adapter = new OAuthAuthorizationAdapter();

            var uriFacade = new OAuth2AuthorizationUriFacade(
                    _provider,
                    clientId,
                    new Uri(callbackUrl),
                    adapter,
                    strategy)
                .AddSecurityParameters(
                    new OAuth2NonceSecurityParameterBundle());

            var accessTokenFacade = new OAuth2AccessCodeFacade(
                _provider,
                clientId,
                clientSecret,
                new Uri(callbackUrl),
                new OAuthAuthorizationAdapter(),
                strategy);

            _web = new OAuth2Web<TResourceProvider>(
                clientId,
                clientSecret,
                _provider,
                callbackHandler,
                uriFacade,
                accessTokenFacade,
                strategy,
                new CryptoStringGenerator());
        }

        /// <summary>
        /// Authenticate a resource owner using the OpenId Connect workflow with default security strategy
        /// </summary>
        /// <param name="clientId">The application's client Id</param>
        /// <param name="clientSecret">The application's client secret</param>
        /// <param name="callbackUrl">The application's registered callback url</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1054:UriParametersShouldNotBeStrings",
            MessageId = "2#")]
        public OpenIdWeb(
            string clientId,
            string clientSecret,
            string callbackUrl) : 
                this(
                    clientId,
                    clientSecret, 
                    callbackUrl, 
                    new OAuthSecurityStrategy(
                        new InMemoryCryptographicParameterRepository(),
                        TimeSpan.FromMinutes(
                            OAuthConfiguration.SecurityParameterTimeoutInMinutes)))
        { }

        /// <summary>
        /// Gets the authorization uri for the Resource Owner to enter his/her credentials
        /// </summary>
        /// <returns>Authorization uri</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")]
        public Task<Uri> GetAuthorizationUriAsync()
        {
            return _web
                .AddScope("openid")
                .GetAuthorizationUriAsync();
        }

        /// <summary>
        /// Exchanges callback uri for access token credentials
        /// </summary>
        /// <param name="responseUri">The received callback uri</param>
        /// <returns>Access token credentials</returns>
        public async Task<JsonWebToken> GetWebTokenAsync(
            Uri responseUri)
        {
            var credentials = await _web.GetAccessTokenAsync(
                        responseUri,
                        true)
                    .ConfigureAwait(false);

            var requestId = _web.GetRequestIdFromResponse(
                responseUri);

            CreateValidator(requestId)?.IsTokenValid(credentials.IdToken);

            _securityStrategy.ClearSecureParameters(requestId);

            return credentials.IdToken;
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
