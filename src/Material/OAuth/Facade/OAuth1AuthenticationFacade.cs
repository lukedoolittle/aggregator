using System;
using System.Threading.Tasks;
using Foundations.Extensions;
using Foundations.HttpClient.Enums;
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
        private readonly IOAuth1Authentication _oauth;
        protected readonly IOAuthSecurityStrategy _securityStrategy;

        public Uri CallbackUri { get; }

        public OAuth1AuthenticationFacade(
            OAuth1ResourceProvider resourceProvider,
            string consumerKey,
            string consumerSecret,
            string callbackUrl,
            IOAuth1Authentication oauth, 
            IOAuthSecurityStrategy securityStrategy)
        {
            _consumerKey = consumerKey;
            _consumerSecret = consumerSecret;
            _resourceProvider = resourceProvider;
            _oauth = oauth;
            _securityStrategy = securityStrategy;
            CallbackUri = new Uri(callbackUrl);
        }

        /// <summary>
        /// Gets the authorization uri for the Resource Owner to enter his/her credentials
        /// </summary>
        /// <param name="userId">Resource owner's Id</param>
        /// <returns>Authorization uri</returns>
        public async Task<Uri> GetAuthorizationUriAsync(string userId)
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
                userId, 
                OAuth1ParameterEnum.OAuthToken.EnumToString(), 
                credentials.OAuthToken);
            _securityStrategy.SetSecureParameter(
                userId,
                OAuth1ParameterEnum.OAuthTokenSecret.EnumToString(),
                credentials.OAuthSecret);

            return authorizationPath;
        }

        /// <summary>
        /// Exchanges intermediate credentials for access token credentials
        /// </summary>
        /// <param name="result">Intermediate credentials received from OAuth1 callback</param>
        /// <param name="secret">The application's OAuth secret</param>
        /// <returns>Access token credentials</returns>
        public async Task<OAuth1Credentials> GetAccessTokenAsync(
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
