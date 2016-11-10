using System.Threading.Tasks;
using Foundations.HttpClient.Enums;
using Material.Contracts;
using Material.Exceptions;
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
            if (!_resourceProvider.GrantTypes.Contains(GrantType.ClientCredentials))
            {
                throw new InvalidGrantTypeException(
                    string.Format(
                        StringResources.GrantTypeNotSupportedException,
                        GrantType.ClientCredentials,
                        _resourceProvider.GetType().Name));
            }

            var token = await _oauth.GetClientAccessToken(
                    _resourceProvider.TokenUrl, 
                    clientId, 
                    clientSecret)
                .ConfigureAwait(false);

            token.SetClientProperties(
                    clientId, 
                    clientSecret)
                .TimestampToken();

            return token;
        }

        public async Task<OAuth2Credentials> GetJsonWebTokenTokenCredentials(
            JsonWebToken jwt,
            string privateKey,
            string clientId)
        {
            if (!_resourceProvider.GrantTypes.Contains(GrantType.JsonWebToken))
            {
                throw new InvalidGrantTypeException(
                    string.Format(
                        StringResources.GrantTypeNotSupportedException,
                        GrantType.JsonWebToken,
                        _resourceProvider.GetType().Name));
            }

            var token = await _oauth.GetJsonWebToken(
                _resourceProvider.TokenUrl,
                jwt,
                privateKey,
                clientId)
                .ConfigureAwait(false);

            token.SetClientProperties(
                    clientId,
                    null)
                .TimestampToken();

            return token;
        }

        public async Task<OAuth2Credentials> GetRefreshedAccessTokenCredentials(
            OAuth2Credentials expiredCredentials)
        {
            if (!_resourceProvider.GrantTypes.Contains(GrantType.RefreshToken))
            {
                throw new InvalidGrantTypeException(
                    string.Format(
                        StringResources.GrantTypeNotSupportedException, 
                        GrantType.RefreshToken,
                        _resourceProvider.GetType().Name));
            }

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

            return token
                .TimestampToken()
                .SetTokenName(expiredCredentials.TokenName)
                .SetClientProperties(
                    expiredCredentials.ClientId,
                    expiredCredentials.ClientSecret)
                .TransferRefreshToken(expiredCredentials.RefreshToken);
        }
    }
}
