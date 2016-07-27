using System;
using System.Threading.Tasks;
using Foundations.Extensions;
using Material.Contracts;
using Material.Enums;
using Material.Infrastructure;
using Material.Infrastructure.Credentials;

namespace Material.OAuth
{
    public class OAuth2AuthenticationFacade : 
        IOAuthFacade<OAuth2Credentials>
    {
        private readonly string _clientId;
        protected readonly string _userId;
        private readonly OAuth2ResourceProvider _resourceProvider;
        private readonly IOAuth2Authentication _oauth;
        protected readonly IOAuthSecurityStrategy _strategy;

        public Uri CallbackUri { get; }

        public OAuth2AuthenticationFacade(
            OAuth2ResourceProvider resourceProvider,
            string clientId,
            string userId,
            string callbackUri,
            IOAuth2Authentication oauth,
            IOAuthSecurityStrategy strategy)
        {
            _clientId = clientId;
            _resourceProvider = resourceProvider;
            CallbackUri = new Uri(callbackUri);
            _oauth = oauth;
            _strategy = strategy;
            _userId = userId;
        }

        public Task<Uri> GetAuthorizationUri()
        {
            var state = _strategy.CreateOrGetSecureParameter(
                _userId,
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

        public async Task<OAuth2Credentials> GetAccessTokenFromCallbackResult(
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
