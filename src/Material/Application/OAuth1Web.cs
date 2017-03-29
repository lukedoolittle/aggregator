using System;
using Material.Domain.Credentials;
using System.Threading.Tasks;
using Material.Authorization;
using Material.Contracts;
using Material.Domain.Core;
using Material.Framework.Enums;
using Material.Framework.Extensions;
using Material.HttpClient.Canonicalizers;
using Material.HttpClient.Cryptography;
using Material.HttpClient.Cryptography.Algorithms;
using Material.Workflow.Callback;
using Material.Workflow.Facade;
using Material.Workflow.Security;

namespace Material.Application
{
    /// <summary>
    /// Authorize a resource owner with the given resource provider using OAuth1a
    /// </summary>
    /// <typeparam name="TResourceProvider">Resource provider to authenticate with</typeparam>
    public class OAuth1Web<TResourceProvider>
        where TResourceProvider: OAuth1ResourceProvider, new()
    {
        private readonly IOAuthAuthorizationUriFacade _uriFacade;
        private readonly IOAuthAccessTokenFacade<OAuth1Credentials> _authorizationFacade;
        private readonly IOAuthSecurityStrategy _securityStrategy;
        private readonly ICryptoStringGenerator _requestIdGenerator;

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1054:UriParametersShouldNotBeStrings", MessageId = "2#")]
        public OAuth1Web(
            string consumerKey,
            string consumerSecret,
            Uri callbackUri,
            IOAuthSecurityStrategy securityStrategy,
            ICryptoStringGenerator cryptoStringGenerator,
            IOAuthAuthorizationAdapter authAdapter,
            ISigningAlgorithm signingAlgorithm,
            IHttpRequestCanonicalizer oauthSignatureCanonicalizer,
            TResourceProvider resourceProvider)
        {
            _securityStrategy = securityStrategy;
            _requestIdGenerator = cryptoStringGenerator;

            var facade = new OAuth1AuthorizationFacade(
                resourceProvider, 
                consumerKey, 
                consumerSecret,
                callbackUri,
                authAdapter,
                securityStrategy,
                signingAlgorithm,
                cryptoStringGenerator,
                oauthSignatureCanonicalizer);

            _uriFacade = facade;
            _authorizationFacade = facade;
        }

        /// <summary>
        /// Authorize a resource owner using the OAuth1a workflow with default security strategy
        /// </summary>
        /// <param name="consumerKey">The application's consumer key</param>
        /// <param name="consumerSecret">The application's consumer secret</param>
        /// <param name="callbackUri">The application's registered callback url</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1054:UriParametersShouldNotBeStrings", MessageId = "2#")]
        public OAuth1Web(
            string consumerKey,
            string consumerSecret,
            string callbackUri) : 
                this (
                    consumerKey, 
                    consumerSecret,
                    new Uri(callbackUri), 
                    new OAuthSecurityStrategy(
                        new InMemoryCryptographicParameterRepository(),
                        QuantfabricConfiguration.SecurityParameterTimeout),
                    new CryptoStringGenerator(),
                    new OAuthAuthorizationAdapter(), 
                    HmacDigestSigningAlgorithm.Sha1Algorithm(),
                    new OAuth1Canonicalizer(),
                    new TResourceProvider())
        { }

        /// <summary>
        /// Gets the authorization uri for the Resource Owner to enter his/her credentials
        /// </summary>
        /// <returns>Authorization uri</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")]
        public Task<Uri> GetAuthorizationUriAsync()
        {
            return _uriFacade.GetAuthorizationUriAsync(
                _requestIdGenerator.CreateRandomString());
        }

        /// <summary>
        /// Exchanges callback uri credentials for access token credentials
        /// </summary>
        /// <param name="responseUri">The received callback uri</param>
        /// <returns>Access token credentials</returns>
        public async Task<OAuth1Credentials> GetAccessTokenAsync(
            Uri responseUri)
        {
            var result = new OAuth1CallbackHandler(
                            _securityStrategy,
                            OAuth1Parameter.OAuthToken.EnumToString())
                        .ParseAndValidateCallback(responseUri);

            var token = await _authorizationFacade.GetAccessTokenAsync(
                    result.Credentials, 
                    result.RequestId)
                .ConfigureAwait(false);

            _securityStrategy.ClearSecureParameters(result.RequestId);

            return token;
        }
    }
}
