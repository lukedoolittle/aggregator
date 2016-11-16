using System.Threading.Tasks;
using Foundations.HttpClient.Cryptography.Keys;
using Foundations.HttpClient.Enums;
using Material.Contracts;
using Material.Infrastructure;
using Material.Infrastructure.Credentials;

namespace Material.OAuth.Facade
{
    public class OAuthClientFacade<TResourceProvider> 
        where TResourceProvider : OAuth2ResourceProvider
    {
        private readonly IOAuth2AuthorizationAdapter _oauth;
        private readonly TResourceProvider _resourceProvider;

        public OAuthClientFacade(
            IOAuth2AuthorizationAdapter oauth, 
            TResourceProvider resourceProvider)
        {
            _oauth = oauth;
            _resourceProvider = resourceProvider;
        }

        public async Task<OAuth2Credentials> GetClientAccessTokenCredentials(
            string clientId,
            string clientSecret)
        {
            _resourceProvider.SetGrant(GrantType.ClientCredentials);

            var token = await _oauth.GetClientAccessToken(
                    _resourceProvider.TokenUrl, 
                    clientId, 
                    clientSecret)
                .ConfigureAwait(false);

            return token.SetClientProperties(
                    clientId, 
                    clientSecret)
                .TimestampToken();
        }

        public async Task<OAuth2Credentials> GetJsonWebTokenTokenCredentials(
            JsonWebToken jwt,
            CryptoKey privateKey,
            string clientId)
        {
            _resourceProvider.SetGrant(GrantType.JsonWebToken);

            var token = await _oauth.GetJsonWebToken(
                _resourceProvider.TokenUrl,
                jwt,
                privateKey,
                clientId)
                .ConfigureAwait(false);

            return token.SetClientProperties(
                    clientId,
                    null)
                .TimestampToken();
        }

        public async Task<OAuth2Credentials> GetRefreshedAccessTokenCredentials(
            OAuth2Credentials expiredCredentials)
        {
            _resourceProvider.SetGrant(GrantType.RefreshToken);

            _resourceProvider.SetClientProperties(
                expiredCredentials.ClientId, 
                expiredCredentials.ClientSecret);

            var token = await _oauth.GetRefreshToken(
                    _resourceProvider.TokenUrl,
                    expiredCredentials.ClientId,
                    expiredCredentials.ClientSecret,
                    expiredCredentials.RefreshToken,
                    _resourceProvider.Headers)
                .ConfigureAwait(false);

            return token.SetClientProperties(
                    expiredCredentials.ClientId,
                    expiredCredentials.ClientSecret)
                .TimestampToken()
                .SetTokenName(expiredCredentials.TokenName)
                .TransferRefreshToken(expiredCredentials.RefreshToken);
        }
    }
}
