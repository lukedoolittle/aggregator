using System.Threading.Tasks;
using Foundations.HttpClient;
using Foundations.HttpClient.Enums;
using Material.Contracts;
using Material.Exceptions;
using Material.Infrastructure.Credentials;

namespace Material.Infrastructure.OAuth
{
    public class OAuthClientFacade : 
        IRefreshTokenFacade, 
        IClientTokenFacade
    {
        private readonly IOAuth2Authentication _oauth;

        public OAuthClientFacade(IOAuth2Authentication oauth)
        {
            _oauth = oauth;
        }

        public async Task<OAuth2Credentials> GetClientAccessTokenCredentials<TResourceProvider>(
            string clientId,
            string clientSecret)
            where TResourceProvider : OAuth2ResourceProvider, new()
        {
            var provider = new TResourceProvider();

            if (!provider.GrantTypes.Contains(GrantTypeEnum.ClientCredentials))
            {
                throw new InvalidGrantTypeException(
                    string.Format(
                        StringResources.GrantTypeNotSupportedException,
                        GrantTypeEnum.ClientCredentials,
                        provider.GetType().Name));
            }

            var token = await _oauth.GetClientAccessToken(
                    provider.TokenUrl, 
                    clientId, 
                    clientSecret)
                .ConfigureAwait(false);

            token.SetClientProperties(
                    clientId, 
                    clientSecret)
                .TimestampToken();

            return token;
        }

        public async Task<OAuth2Credentials> GetJsonWebTokenTokenCredentials<TResourceProvider>(
            JsonWebToken jwt,
            IJWTSigningFactory signingFactory,
            string privateKey,
            string clientId,
            TResourceProvider provider = null)
            where TResourceProvider : OAuth2ResourceProvider, new()
        {
            if (provider == null)
            {
                provider = new TResourceProvider();
            }

            if (!provider.GrantTypes.Contains(GrantTypeEnum.JsonWebToken))
            {
                throw new InvalidGrantTypeException(
                    string.Format(
                        StringResources.GrantTypeNotSupportedException,
                        GrantTypeEnum.JsonWebToken,
                        provider.GetType().Name));
            }

            var token = await _oauth.GetJsonWebToken(
                provider.TokenUrl,
                jwt,
                signingFactory,
                privateKey,
                clientId)
                .ConfigureAwait(false);

            token.SetClientProperties(
                    clientId,
                    null)
                .TimestampToken();

            return token;
        }

        public async Task<OAuth2Credentials> GetRefreshedAccessTokenCredentials<TResourceProvider>(
            OAuth2Credentials expiredCredentials,
            TResourceProvider provider = null)
            where TResourceProvider : OAuth2ResourceProvider, new()
        {
            if (provider == null)
            {
                provider = new TResourceProvider();
            }

            if (!provider.GrantTypes.Contains(GrantTypeEnum.RefreshToken))
            {
                throw new InvalidGrantTypeException(
                    string.Format(
                        StringResources.GrantTypeNotSupportedException, 
                        GrantTypeEnum.RefreshToken, 
                        provider.GetType().Name));
            }

            provider.SetClientProperties(
                expiredCredentials.ClientId, 
                expiredCredentials.ClientSecret);

            var token = await _oauth.GetRefreshToken(
                    provider.TokenUrl,
                    expiredCredentials.ClientId,
                    expiredCredentials.ClientSecret,
                    expiredCredentials.RefreshToken,
                    provider.Headers)
                .ConfigureAwait(false);

            token.TimestampToken();

            return token
                .SetTokenName(expiredCredentials.TokenName)
                .SetClientProperties(
                    expiredCredentials.ClientId,
                    expiredCredentials.ClientSecret)
                .TransferRefreshToken(expiredCredentials.RefreshToken);
        }
    }
}
