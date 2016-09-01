using System;
using System.Threading.Tasks;
using Foundations.Extensions;
using Foundations.HttpClient.Enums;
using Material.Contracts;
using Material.Infrastructure;
using Material.Infrastructure.Credentials;
using Material.Infrastructure.OAuth;

namespace Material.Facades
{
    /// <summary>
    /// Authenticates a resource owner with the given resource provider using OAuth1a
    /// </summary>
    /// <typeparam name="TResourceProvider">Resource provider to authenticate with</typeparam>
    public class OAuth1Web<TResourceProvider>
        where TResourceProvider: OAuth1ResourceProvider, new()
    {
        private readonly IOAuthFacade<OAuth1Credentials> _authFacade;

        /// <summary>
        /// Authenticates a resource owner using the OAuth1a workflow
        /// </summary>
        /// <param name="consumerKey">The application's consumer key</param>
        /// <param name="consumerSecret">The application's consumer secret</param>
        /// <param name="callbackUrl">The application's registered callback url</param>
        /// <param name="securityStrategy">The security strategy to use for token and secret handling</param>
        public OAuth1Web(
            string consumerKey,
            string consumerSecret,
            string callbackUrl,
            IOAuthSecurityStrategy securityStrategy)
        {
            var handler = new OAuth1CallbackHandler(
                securityStrategy,
                OAuth1ParameterEnum.OAuthToken.EnumToString());

            _authFacade = new OAuth1AuthenticationFacade(
                new TResourceProvider(), 
                consumerKey, 
                consumerSecret,
                callbackUrl,
                new OAuth1Authentication(),
                securityStrategy, 
                handler);
        }

        /// <summary>
        /// Authenticates a resource owner using the OAuth1a workflow with default security strategy
        /// </summary>
        /// <param name="consumerKey">The application's consumer key</param>
        /// <param name="consumerSecret">The application's consumer secret</param>
        /// <param name="callbackUrl">The application's registered callback url</param>
        public OAuth1Web(
            string consumerKey,
            string consumerSecret,
            string callbackUrl)
        {
            var securityStrategy = new OAuthSecurityStrategy(
                new InMemoryCryptographicParameterRepository(),
                TimeSpan.FromMinutes(2));

            var handler = new OAuth1CallbackHandler(
                securityStrategy,
                OAuth1ParameterEnum.OAuthToken.EnumToString());

            _authFacade = new OAuth1AuthenticationFacade(
                new TResourceProvider(),
                consumerKey,
                consumerSecret,
                callbackUrl,
                new OAuth1Authentication(),
                securityStrategy,
                handler);
        }

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
        /// Convert a callback uri into intermediate OAuth2Credentials
        /// </summary>
        /// <param name="responseUri">The received callback uri</param>
        /// <param name="userId">Resource owner's Id</param>
        /// <returns>Intermediate OAuth1 credentials</returns>
        public OAuth1Credentials ParseAndValidateCallback(
            Uri responseUri,
            string userId)
        {
            return _authFacade.ParseAndValidateCallback(responseUri, userId);
        }

        /// <summary>
        /// Exchanges intermediate credentials for access token credentials
        /// </summary>
        /// <param name="result">Intermediate credentials received from OAuth2 callback</param>
        /// <param name="secret">The application's client secret</param>
        /// <returns>Access token credentials</returns>
        public Task<OAuth1Credentials> GetAccessTokenAsync(
            OAuth1Credentials result,
            string secret)
        {
            return _authFacade.GetAccessTokenAsync(result, secret);
        }
    }
}
