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

namespace Material.Application
{
    public class OpenIdWeb<TResourceProvider> : OAuth2Web<TResourceProvider>
        where TResourceProvider : OpenIdResourceProvider, new()
    {
        public OpenIdWeb(
            string clientId, 
            string clientSecret, 
            TResourceProvider resourceProvider, 
            IOAuthCallbackHandler<OAuth2Credentials> callbackHandler, 
            IOAuthAuthorizationUriFacade uriFacade, 
            IOAuthAccessTokenFacade<OAuth2Credentials> accessTokenFacade, 
            IOAuthSecurityStrategy securityStrategy, 
            ICryptoStringGenerator idGenerator) : 
                base(
                    clientId, 
                    clientSecret, 
                    resourceProvider,
                    callbackHandler, 
                    uriFacade, 
                    accessTokenFacade, 
                    securityStrategy, 
                    idGenerator)
        {}

        /// <summary>
        /// Authenticate a resource owner using the OpenId Connect workflow
        /// </summary>
        /// <param name="clientId">The application's client Id</param>
        /// <param name="clientSecret">The application's client secret</param>
        /// <param name="callbackUri">The application's registered callback url</param>
        /// <param name="strategy"></param>
        /// <param name="resourceProvider"></param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1054:UriParametersShouldNotBeStrings", MessageId = "2#")]
        public OpenIdWeb(
            string clientId, 
            string clientSecret, 
            string callbackUri, 
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
                            new Uri(callbackUri),
                            new OAuthAuthorizationAdapter(),
                            strategy)
                        .AddSecurityParameters(
                            new OAuth2NonceSecurityParameterBundle()),
                    new OAuth2AccessCodeFacade(
                        resourceProvider,
                        clientId,
                        clientSecret,
                        new Uri(callbackUri),
                        new OAuthAuthorizationAdapter(),
                        strategy),
                    strategy,
                    new CryptoStringGenerator())
        { }

        /// <summary>
        /// Authenticate a resource owner using the OpenId Connect workflow
        /// </summary>
        /// <param name="clientId">The application's client Id</param>
        /// <param name="clientSecret">The application's client secret</param>
        /// <param name="callbackUri">The application's registered callback url</param>
        /// <param name="resourceProvider"></param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1054:UriParametersShouldNotBeStrings", MessageId = "2#")]
        public OpenIdWeb(
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
        /// Authenticate a resource owner using the OpenId Connect workflow 
        /// </summary>
        /// <param name="clientId">The application's client Id</param>
        /// <param name="clientSecret">The application's client secret</param>
        /// <param name="callbackUri">The application's registered callback url</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1054:UriParametersShouldNotBeStrings", MessageId = "2#")]
        public OpenIdWeb(
            string clientId,
            string clientSecret,
            string callbackUri) :
                this(
                    clientId,
                    clientSecret,
                    callbackUri,
                    new TResourceProvider())
        { }

        /// <summary>
        /// Gets the authorization uri for the Resource Owner to enter his/her credentials
        /// </summary>
        /// <returns>Authorization uri</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")]
        public override Task<Uri> GetAuthorizationUriAsync()
        {
            AddScope(OpenIdResourceProvider.OpenIdScope);

            return base.GetAuthorizationUriAsync();
        }

        /// <summary>
        /// Exchanges callback uri for access token credentials
        /// </summary>
        /// <param name="responseUri">The received callback uri</param>
        /// <returns>Access token credentials</returns>
        public async Task<JsonWebToken> GetWebTokenAsync(
            Uri responseUri)
        {
            var credentials = await GetAccessTokenAsync(
                    responseUri, 
                    true)
                .ConfigureAwait(false);

            var requestId = CallbackHandler
                .ParseAndValidateCallback(
                    responseUri)
                .RequestId;

            CreateValidator(requestId).IsTokenValid(credentials.IdToken);

            SecurityStrategy.ClearSecureParameters(requestId);

            return credentials.IdToken;
        }

        private IJsonWebTokenAuthenticationValidator CreateValidator(
            string requestId)
        {
            return new CompositeJsonWebTokenAuthenticationValidator()
                .AddValidator(new JsonWebTokenAlgorithmValidator())
                .AddValidator(new JsonWebTokenExpirationValidator())
                .AddValidator(new JsonWebTokenAudienceValidator(
                    ClientId))
                .AddValidator(new JsonWebTokenIssuerValidator(
                    ResourceProvider.ValidIssuers))
                .AddValidator(new JsonWebTokenNonceValidator(
                    SecurityStrategy,
                    requestId))
                .AddValidator(new DiscoveryJsonWebTokenSignatureValidator(
                    ResourceProvider.OpenIdDiscoveryUrl))
                .ThrowIfInvalid();
        }
    }
}
