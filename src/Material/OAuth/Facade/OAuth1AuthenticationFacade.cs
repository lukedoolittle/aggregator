using System;
using System.Threading.Tasks;
using Foundations.Extensions;
using Material.Contracts;
using Material.Enums;
using Material.Infrastructure;
using Material.Infrastructure.Credentials;

namespace Material.OAuth
{
    public class OAuth1AuthenticationFacade : 
        IOAuthFacade<OAuth1Credentials>
    {
        private readonly OAuth1ResourceProvider _resourceProvider;
        private readonly string _consumerKey;
        private readonly string _consumerSecret;
        protected readonly string _userId;
        private readonly IOAuth1Authentication _oauth;
        protected readonly IOAuthSecurityStrategy _securityStrategy;

        public Uri CallbackUri { get; }

        public OAuth1AuthenticationFacade(
            OAuth1ResourceProvider resourceProvider,
            string consumerKey,
            string consumerSecret,
            string callbackUrl,
            string userId,
            IOAuth1Authentication oauth, 
            IOAuthSecurityStrategy securityStrategy)
        {
            _consumerKey = consumerKey;
            _consumerSecret = consumerSecret;
            _resourceProvider = resourceProvider;
            _oauth = oauth;
            _securityStrategy = securityStrategy;
            _userId = userId;
            CallbackUri = new Uri(callbackUrl);
        }

        public async Task<Uri> GetAuthorizationUri()
        {
            var credentials =
                await _oauth
                    .GetRequestToken(
                        _resourceProvider.RequestUrl,
                        _consumerKey,
                        _consumerSecret,
                        _resourceProvider.ParameterType,
                        CallbackUri)
                    .ConfigureAwait(false);

            var authorizationPath =
                _oauth.GetAuthorizationUri(
                    credentials.OAuthToken,
                    _resourceProvider.AuthorizationUrl);

            _securityStrategy.SetSecureParameter(
                _userId, 
                OAuth1ParameterEnum.OAuthToken.EnumToString(), 
                credentials.OAuthToken);
            _securityStrategy.SetSecureParameter(
                _userId,
                OAuth1ParameterEnum.OAuthTokenSecret.EnumToString(),
                credentials.OAuthSecret);

            return authorizationPath;
        }

        public async Task<OAuth1Credentials> GetAccessTokenFromCallbackResult(
            OAuth1Credentials result,
            string secret)
        {
            var token = await _oauth
                .GetAccessToken(
                    _resourceProvider.TokenUrl,
                    _consumerKey,
                    _consumerSecret,
                    result.OAuthToken,
                    secret,
                    result.Verifier,
                    _resourceProvider.ParameterType,
                    result.AdditionalParameters)
                .ConfigureAwait(false);

            return token
                .SetConsumerProperties(
                    _consumerKey,
                    _consumerSecret)
                .SetParameterHandling(
                    _resourceProvider.ParameterType);
        }
    }
}
