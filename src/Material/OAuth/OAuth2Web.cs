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
    public class OAuth2Web<TResourceProvider>
        where TResourceProvider : OAuth2ResourceProvider, new()
    {
        private readonly IOAuthFacade<OAuth2Credentials> _authFacade;
        private readonly TResourceProvider _resourceProvider = 
            new TResourceProvider();
        /// <summary>
        /// Authenticates a resource owner using the OAuth2 workflow with default security strategy
        /// </summary>
        /// <param name="clientId">The application's client Id</param>
        /// <param name="callbackUri">The application's registered callback url</param>
        /// <param name="strategy">The security strategy to use for "state" handling</param>
        public OAuth2Web(
            string clientId,
            string callbackUri,
            IOAuthSecurityStrategy strategy)
        {
            var handler = new OAuth2QueryCallbackHandler(
                strategy,
                OAuth2ParameterEnum.State.EnumToString());

            //NOTE: you could create a token workflow version of this
            //but why???
            //
            //var handler = new OAuth2QueryCallbackHandler(
            //_strategy,
            //OAuth2ParameterEnum.State.EnumToString(),
            //userId);

            _authFacade = new OAuth2AuthenticationFacade(
                _resourceProvider,
                clientId,
                callbackUri,
                new OAuth2Authentication(),
                strategy,
                handler);
        }

        /// <summary>
        /// Authenticates a resource owner using the OAuth2 workflow with default security strategy
        /// </summary>
        /// <param name="clientId">The application's client Id</param>
        /// <param name="callbackUri">The application's registered callback url</param>
        public OAuth2Web(
            string clientId,
            string callbackUri)
        {
            var strategy = new OAuthSecurityStrategy(
                new InMemoryCryptographicParameterRepository(),
                TimeSpan.FromMinutes(2));

            var handler = new OAuth2QueryCallbackHandler(
                strategy,
                OAuth2ParameterEnum.State.EnumToString());

            _authFacade = new OAuth2AuthenticationFacade(
                    _resourceProvider,
                    clientId,
                    callbackUri,
                    new OAuth2Authentication(),
                    strategy,
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
        public OAuth2Credentials ParseAndValidateCallback(
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
        public Task<OAuth2Credentials> GetAccessTokenAsync(
            OAuth2Credentials result,
            string secret)
        {
            return _authFacade.GetAccessTokenAsync(result, secret);
        }

        /// <summary>
        /// Adds scope to be requested with OAuth2 authentication
        /// </summary>
        /// <typeparam name="TRequest">The request type scope is needed for</typeparam>
        /// <returns>The current instance</returns>
        public OAuth2Web<TResourceProvider> AddScope<TRequest>()
            where TRequest : OAuthRequest, new()
        {
            _resourceProvider.AddRequestScope<TRequest>();

            return this;
        }
    }
}
