using System;
using System.Threading.Tasks;
using Material.Contracts;
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
        private string _oauthToken;
        private string _oauthSecret;
        private readonly IOAuth1Authentication _oauth;

        public Uri CallbackUri { get; }

        public OAuth1AuthenticationFacade(
            OAuth1ResourceProvider resourceProvider,
            string consumerKey,
            string consumerSecret,
            string callbackUrl,
            IOAuth1Authentication oauth)
        {
            _consumerKey = consumerKey;
            _consumerSecret = consumerSecret;
            _resourceProvider = resourceProvider;
            _oauth = oauth;
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

            _oauthToken = credentials.OAuthToken;
            _oauthSecret = credentials.OAuthSecret;
            return authorizationPath;
        }

        public async Task<OAuth1Credentials> GetAccessTokenFromCallbackResult(
            OAuth1Credentials result)
        {
            var token = await _oauth
                .GetAccessToken(
                    _resourceProvider.TokenUrl,
                    _consumerKey,
                    _consumerSecret,
                    _oauthToken,
                    _oauthSecret,
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
