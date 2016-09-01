using System;
using System.Threading.Tasks;
using Foundations.Extensions;
using Foundations.HttpClient.Enums;
using Material.Contracts;
using Material.Infrastructure;
using Material.Infrastructure.Credentials;

namespace Material.Infrastructure.OAuth
{
    public class OAuth2AuthenticationFacade : 
        IOAuthFacade<OAuth2Credentials>
    {
        private readonly string _clientId;
        private readonly OAuth2ResourceProvider _resourceProvider;
        private readonly IOAuth2Authentication _oauth;
        private readonly IOAuthCallbackHandler<OAuth2Credentials> _handler;
        protected readonly IOAuthSecurityStrategy _strategy;

        public Uri CallbackUri { get; }

        public OAuth2AuthenticationFacade(
            OAuth2ResourceProvider resourceProvider,
            string clientId,
            string callbackUri,
            IOAuth2Authentication oauth,
            IOAuthSecurityStrategy strategy,
            IOAuthCallbackHandler<OAuth2Credentials> handler)
        {
            _clientId = clientId;
            _resourceProvider = resourceProvider;
            CallbackUri = new Uri(callbackUri);
            _oauth = oauth;
            _strategy = strategy;
            _handler = handler;
        }

        /// <summary>
        /// Gets the authorization uri for the Resource Owner to enter his/her credentials
        /// </summary>
        /// <param name="userId">Resource owner's Id</param>
        /// <returns>Authorization uri</returns>
        public Task<Uri> GetAuthorizationUriAsync(string userId)
        {
            var state = _strategy.CreateOrGetSecureParameter(
                userId,
                OAuth2ParameterEnum.State.EnumToString());

            var authorizationPath =
                _oauth.GetAuthorizationUri(
                    _resourceProvider.AuthorizationUrl,
                    _clientId,
                    _resourceProvider.Scope,
                    CallbackUri,
                    state,
                    _resourceProvider.Flow,
                    _resourceProvider.Parameters);

            return Task.FromResult(authorizationPath);
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
            return _handler.ParseAndValidateCallback(
                responseUri,
                userId);
        }

        /// <summary>
        /// Exchanges intermediate credentials for access token credentials
        /// </summary>
        /// <param name="result">Intermediate credentials received from OAuth2 callback</param>
        /// <param name="secret">The application's client secret</param>
        /// <returns>Access token credentials</returns>
        public async Task<OAuth2Credentials> GetAccessTokenAsync(
            OAuth2Credentials result,
            string secret)
        {
            _resourceProvider.SetClientProperties(
                _clientId,
                secret);

            var accessToken = await _oauth.GetAccessToken(
                _resourceProvider.TokenUrl,
                _clientId,
                secret,
                CallbackUri,
                result.Code,
                _resourceProvider.Scope,
                _resourceProvider.Headers)
                .ConfigureAwait(false);

            return accessToken
                .SetTokenName(_resourceProvider.TokenName)
                .SetClientProperties(
                    _clientId, 
                    secret);
        }
    }
}
