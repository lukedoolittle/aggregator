using System;
using System.Threading.Tasks;
using Foundations.Extensions;
using Foundations.HttpClient.Canonicalizers;
using Foundations.HttpClient.Cryptography;
using Foundations.HttpClient.Cryptography.Algorithms;
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
        private readonly ICryptoStringGenerator _idGenerator;

        /// <summary>
        /// Authorize a resource owner using the OAuth1a workflow
        /// </summary>
        /// <param name="consumerKey">The application's consumer key</param>
        /// <param name="consumerSecret">The application's consumer secret</param>
        /// <param name="callbackUrl">The application's registered callback url</param>
        /// <param name="securityStrategy">The security strategy to use for token and secret handling</param>
        /// <param name="idGenerator"></param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1054:UriParametersShouldNotBeStrings", MessageId = "2#")]
        public OAuth1Web(
            string consumerKey,
            string consumerSecret,
            string callbackUrl,
            IOAuthSecurityStrategy securityStrategy,
            ICryptoStringGenerator idGenerator)
        {
            _securityStrategy = securityStrategy;
            _idGenerator = idGenerator;

            var facade = new OAuth1AuthorizationFacade(
                new TResourceProvider(), 
                consumerKey, 
                consumerSecret,
                new Uri(callbackUrl),
                new OAuthAuthorizationAdapter(),
                securityStrategy,
                HmacDigestSigningAlgorithm.Sha1Algorithm(),
                new CryptoStringGenerator(),
                new OAuth1Canonicalizer());

            _uriFacade = facade;
            _authorizationFacade = facade;
        }

        /// <summary>
        /// Authorize a resource owner using the OAuth1a workflow with default security strategy
        /// </summary>
        /// <param name="consumerKey">The application's consumer key</param>
        /// <param name="consumerSecret">The application's consumer secret</param>
        /// <param name="callbackUrl">The application's registered callback url</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1054:UriParametersShouldNotBeStrings", MessageId = "2#")]
        public OAuth1Web(
            string consumerKey,
            string consumerSecret,
            string callbackUrl) : 
                this (
                    consumerKey, 
                    consumerSecret,
                    callbackUrl, 
                    new OAuthSecurityStrategy(
                        new InMemoryCryptographicParameterRepository(),
                        TimeSpan.FromMinutes(
                            OAuthConfiguration.SecurityParameterTimeoutInMinutes)),
                    new CryptoStringGenerator())
        { }

        /// <summary>
        /// Gets the authorization uri for the Resource Owner to enter his/her credentials
        /// </summary>
        /// <returns>Authorization uri</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")]
        public Task<Uri> GetAuthorizationUriAsync()
        {
            return _uriFacade.GetAuthorizationUriAsync(
                _idGenerator.CreateRandomString());
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
