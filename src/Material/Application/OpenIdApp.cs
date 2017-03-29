using System;
using Material.Domain.Credentials;
using System.Threading.Tasks;
using Material.Authentication.Validation;
using Material.Contracts;
using Material.Domain.Core;
using Material.Framework.Enums;
using Material.HttpClient.Cryptography;
using Material.HttpClient.Cryptography.Algorithms;
using Material.Workflow.Security;

namespace Material.Application
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

        /// <summary>
        /// Authenticates a user with the OpenId Connect workflow
        /// </summary>
        /// <param name="clientId">The application's clientId</param>
        /// <param name="callbackUri">The application's registered callback url</param>
        /// <param name="browserType">The type of authorization interface requested</param>
        /// <param name="securityStrategy">Strategy for handling temporary parameters in the workflow exchange</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1054:UriParametersShouldNotBeStrings", MessageId = "1#")]
        public OpenIdApp(
            string clientId,
            string callbackUri,
            AuthorizationInterface browserType,
            IOAuthSecurityStrategy securityStrategy) : 
                base(
                    clientId,
                    callbackUri,
                    browserType,
                    securityStrategy)
        { }

        /// <summary>
        /// Authenticates a user with the OpenId Connect workflow
        /// </summary>
        /// <param name="clientId">The application's clientId</param>
        /// <param name="callbackUri">The application's registered callback url</param>
        /// <param name="browserType">The type of authorization interface requested</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1054:UriParametersShouldNotBeStrings", MessageId = "1#")]
        public OpenIdApp(
            string clientId,
            string callbackUri,
            AuthorizationInterface browserType) :
                base(
                    clientId,
                    callbackUri,
                    browserType)
        { }

        /// <summary>
        /// Authenticates a user with the OpenId Connect workflow
        /// </summary>
        /// <param name="clientId">The application's clientId</param>
        /// <param name="callbackUri">The application's registered callback url</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1054:UriParametersShouldNotBeStrings", MessageId = "1#")]
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
                RequestIdGenerator.CreateRandomString());
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
                RequestIdGenerator.CreateRandomString());
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

            if ((BrowserType == AuthorizationInterface.Dedicated ||
                 BrowserType == AuthorizationInterface.SecureEmbedded) &&
                 Provider.SupportsPkce)
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

                SecurityStrategy.ClearSecureParameters(requestId);

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

                SecurityStrategy.ClearSecureParameters(requestId);

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
                    requestId,
                    false)
                .ConfigureAwait(false);

            CreateValidator(requestId).IsTokenValid(credentials?.IdToken);

            SecurityStrategy.ClearSecureParameters(requestId);

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
                    ClientId))
                .AddValidator(new JsonWebTokenIssuerValidator(
                    Provider.ValidIssuers))
                .AddValidator(new JsonWebTokenNonceValidator(
                    SecurityStrategy,
                    requestId))
                .AddValidator(new DiscoveryJsonWebTokenSignatureValidator(
                    Provider.OpenIdDiscoveryUrl))
                .ThrowIfInvalid();
        }
    }
}
