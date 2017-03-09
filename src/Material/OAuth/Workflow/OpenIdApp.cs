using System;
using System.Threading.Tasks;
using Foundations.HttpClient.Cryptography;
using Foundations.HttpClient.Cryptography.Algorithms;
using Foundations.HttpClient.Enums;
using Material.Contracts;
using Material.Enums;
using Material.Infrastructure;
using Material.Infrastructure.Credentials;
using Material.OAuth.Authentication;
using Material.OAuth.Security;

namespace Material.OAuth.Workflow
{
    public class OpenIdApp<TResourceProvider> : 
        OAuth2App<TResourceProvider>
        where TResourceProvider : OpenIdResourceProvider, new()
    {
        public OpenIdApp(
            string clientId, 
            Uri callbackUri, 
            IOAuthAuthorizerUIFactory uiFactory, 
            IOAuthSecurityStrategy securityStrategy, 
            IOAuthCallbackHandler<OAuth2Credentials> callbackHandler, 
            IOAuthAuthorizationAdapter authAdapter,
            ICryptoStringGenerator requestIdGenerator,
            IAuthorizationUISelector authorizerSelector,
            TResourceProvider provider, 
            AuthorizationInterface browserType) : 
                base(
                    clientId, 
                    callbackUri, 
                    uiFactory, 
                    securityStrategy, 
                    callbackHandler, 
                    authAdapter,
                    requestIdGenerator,
                    authorizerSelector,
                    provider,
                    browserType)
        { }

        public OpenIdApp(
            string clientId,
            string callbackUri,
            IOAuthSecurityStrategy securityStrategy) : 
                base(
                    clientId,
                    callbackUri, 
                    securityStrategy)
        { }

        /// <summary>
        /// Authenticates a user with the OAuth2 workflow
        /// </summary>
        /// <param name="clientId">The application's clientId</param>
        /// <param name="callbackUri">The application's registered callback url</param>
        public OpenIdApp(
            string clientId, 
            string callbackUri) : 
                base(
                    clientId, 
                    callbackUri)
        { }

        /// <summary>
        /// Authenticates a user with the OAuth2 workflow
        /// </summary>
        /// <returns>Valid OAuth2 credentials</returns>
        public Task<JsonWebToken> GetWebTokenAsync()
        {
            return GetWebTokenWithRequestIdAsync(
                _requestIdGenerator.CreateRandomString());
        }

        /// <summary>
        /// Authenticates a user with the OAuth2 code workflow
        /// </summary>
        /// <param name="clientSecret">The client secret for the application</param>
        /// <returns>Valid OAuth2 credentials</returns>
        public Task<JsonWebToken> GetWebTokenAsync(string clientSecret)
        {
            return GetWebTokenWithRequestIdAsync(
                clientSecret,
                _requestIdGenerator.CreateRandomString());
        }

        /// <summary>
        /// Authenticates a user with the OAuth2 code workflow
        /// </summary>
        /// <param name="requestId">The unique ID of the request NOTE THIS MUST AT LEAST BE UNIQUE PER USER</param>
        /// <returns>Valid OAuth2 credentials</returns>
        public async Task<JsonWebToken> GetWebTokenWithRequestIdAsync(
            string requestId)
        {
            if (requestId == null) throw new ArgumentNullException(nameof(requestId));

            AddScope(OpenIdResourceProvider.OpenIdScope);

            if ((_browserType == AuthorizationInterface.Dedicated ||
                 _browserType == AuthorizationInterface.SecureEmbedded) &&
                 _provider.SupportsPkce)
            {
                var credentials = await GetCredentialsAsync(
                        OAuth2FlowType.AccessCode,
                        OAuth2ResponseType.Code,
                        CreateUriFacade(
                            new OAuth2NonceSecurityParameterBundle(),
                            new OAuth2Sha256PkceSecurityParameterBundle(
                                DigestSigningAlgorithm.Sha256Algorithm())),
                        CreateCodeFacade(
                                new OAuth2PkceVerifierSecurityParameterBundle()),
                        requestId,
                        false)
                    .ConfigureAwait(false);

                CreateValidator(requestId).IsTokenValid(credentials.IdToken);

                _securityStrategy.ClearSecureParameters(requestId);

                return credentials?.IdToken;
            }
            else
            {
                var credentials = await GetCredentialsAsync(
                        OAuth2FlowType.Implicit,
                        OAuth2ResponseType.IdTokenToken,
                        CreateUriFacade(
                            new OAuth2NonceSecurityParameterBundle()),
                        CreateTokenFacade(),
                        requestId,
                        false)
                    .ConfigureAwait(false);

                CreateValidator(requestId).IsTokenValid(credentials?.IdToken);

                _securityStrategy.ClearSecureParameters(requestId);

                return credentials?.IdToken;
            }
        }

        /// <summary>
        /// Authenticates a user with the OAuth2 code workflow
        /// </summary>
        /// <param name="requestId">The unique ID of the request NOTE THIS MUST AT LEAST BE UNIQUE PER USER</param>
        /// <param name="clientSecret">The client secret for the application</param>
        /// <returns>Valid OAuth2 credentials</returns>
        public async Task<JsonWebToken> GetWebTokenWithRequestIdAsync(
            string clientSecret,
            string requestId)
        {
            if (clientSecret == null) throw new ArgumentNullException(nameof(clientSecret));
            if (requestId == null) throw new ArgumentNullException(nameof(requestId));

            AddScope(OpenIdResourceProvider.OpenIdScope);

            var credentials = await GetCredentialsAsync(
                    OAuth2FlowType.AccessCode,
                    OAuth2ResponseType.Code,
                    CreateUriFacade(
                        new OAuth2NonceSecurityParameterBundle()),
                    CreateCodeFacade(clientSecret),
                    requestId)
                .ConfigureAwait(false);

            CreateValidator(requestId).IsTokenValid(credentials?.IdToken);

            _securityStrategy.ClearSecureParameters(requestId);

            return credentials?.IdToken;
        }

        //TODO: this should be populated from the configuration
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
