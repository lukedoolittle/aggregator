using System;
using System.Security;
using System.Threading.Tasks;
using Foundations.Extensions;
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
                    new OAuth2StateSecurityParameterBundle())
                .AddSecurityParameters(
                    new OAuth2NonceSecurityParameterBundle());

            var accessTokenFacade = new OAuth2AccessCodeFacade(
                _provider,
                clientId,
                new Uri(callbackUrl),
                new OAuthAuthorizationAdapter(),
                strategy);

            _web = new OAuth2Web<TResourceProvider>(
                clientSecret,
                _provider,
                callbackHandler,
                uriFacade,
                accessTokenFacade);
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
        /// <param name="userId">Resource owner's Id</param>
        /// <returns>Authorization uri</returns>
        public Task<Uri> GetAuthorizationUriAsync(string userId)
        {
            _web.AddScope("openid");

            return _web.GetAuthorizationUriAsync(userId);
        }

        /// <summary>
        /// Exchanges callback uri for access token credentials
        /// </summary>
        /// <param name="userId">Resource owner's Id</param>
        /// <param name="responseUri">The received callback uri</param>
        /// <returns>Access token credentials</returns>
        public async Task<JsonWebToken> GetWebTokenAsync(
            Uri responseUri,
            string userId)
        {
            var credentials = await _web.GetAccessTokenAsync(
                    responseUri, 
                    userId)
                .ConfigureAwait(false);

            var nonce = _securityStrategy.CreateOrGetSecureParameter(
                userId,
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
