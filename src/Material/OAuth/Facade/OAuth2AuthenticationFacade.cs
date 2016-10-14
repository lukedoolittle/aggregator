using System;
using System.Threading.Tasks;
using Foundations.Extensions;
using Foundations.HttpClient.Enums;
using Material.Contracts;
using Material.Infrastructure.Credentials;

namespace Material.Infrastructure.OAuth
{
    public class OAuth2AuthenticationFacade : 
        IOAuthFacade<OAuth2Credentials>
    {
        private readonly string _clientId;
        private readonly OAuth2ResourceProvider _resourceProvider;
        private readonly IOAuth2AuthenticationAdapter _oauth;
        private readonly string _callbackUri;
        protected readonly IOAuthSecurityStrategy _strategy;

        public OAuth2AuthenticationFacade(
            OAuth2ResourceProvider resourceProvider,
            string clientId,
            string callbackUri,
            IOAuth2AuthenticationAdapter oauth,
            IOAuthSecurityStrategy strategy)
        {
            _clientId = clientId;
            _resourceProvider = resourceProvider;
            _callbackUri = callbackUri;
            _oauth = oauth;
            _strategy = strategy;
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
                    new Uri(_callbackUri), 
                    state,
                    _resourceProvider.Flow,
                    _resourceProvider.Parameters);

            return Task.FromResult(authorizationPath);
        }

        /// <summary>
        /// Exchanges intermediate credentials for access token credentials
        /// </summary>
        /// <param name="intermediateResult">Intermediate credentials received from OAuth2 callback</param>
        /// <param name="clientSecret">The application's client secret</param>
        /// <returns>Access token credentials</returns>
        public async Task<OAuth2Credentials> GetAccessTokenAsync(
            OAuth2Credentials intermediateResult,
            string clientSecret)
        {
            _resourceProvider.SetClientProperties(
                _clientId,
                clientSecret);

            var accessToken = await _oauth.GetAccessToken(
                _resourceProvider.TokenUrl,
                _clientId,
                clientSecret,
                new Uri(_callbackUri),
                intermediateResult.Code,
                _resourceProvider.Scope,
                _resourceProvider.Headers)
                .ConfigureAwait(false);

            return accessToken
                .SetTokenName(_resourceProvider.TokenName)
                .SetClientProperties(
                    _clientId,
                    clientSecret);
        }
    }
}
