using System.Threading.Tasks;
using Material.Contracts;
using Material.Domain.Core;
using Material.Domain.Credentials;
using Material.Framework.Enums;
using Material.HttpClient.Authenticators;
using Material.HttpClient.Cryptography;
using Material.HttpClient.Cryptography.Keys;
using Material.Workflow.AuthenticatorParameters;

namespace Material.Workflow.Facade
{
    public class OAuthClientFacade<TResourceProvider> 
        where TResourceProvider : OAuth2ResourceProvider
    {
        private readonly IOAuthAuthorizationAdapter _oauth;
        private readonly TResourceProvider _resourceProvider;

        public OAuthClientFacade(
            IOAuthAuthorizationAdapter oauth, 
            TResourceProvider resourceProvider)
        {
            _oauth = oauth;
            _resourceProvider = resourceProvider;
        }

        public Task<OAuth2Credentials> GetClientAccessTokenCredentials(
            string clientId,
            string clientSecret)
        {
            _resourceProvider.SetGrant(GrantType.ClientCredentials);

            var builder = new AuthenticatorBuilder()
                .AddParameter(new OAuth2ClientCredentials(
                    clientId, 
                    clientSecret));

            return GetAccessToken(
                clientId, 
                clientSecret, 
                builder);
        }

        public  Task<OAuth2Credentials> GetJsonWebTokenTokenCredentials(
            JsonWebToken jwt,
            CryptoKey privateKey,
            string clientId)
        {
            _resourceProvider.SetGrant(GrantType.JsonWebToken);

            var builder = new AuthenticatorBuilder()
                .AddParameter(new OAuth2Assertion(
                    jwt,
                    privateKey,
                    new JsonWebTokenSignerFactory()));

            if (clientId != null)
            {
                builder.AddParameter(new OAuth2ClientId(clientId));
            }

            return GetAccessToken(
                clientId,
                null, 
                builder);
        }

        public async Task<OAuth2Credentials> GetRefreshedAccessTokenCredentials(
            OAuth2Credentials expiredCredentials)
        {
            _resourceProvider.SetGrant(GrantType.RefreshToken);

            _resourceProvider.SetClientProperties(
                expiredCredentials.ClientId, 
                expiredCredentials.ClientSecret);

            var builder = new AuthenticatorBuilder()
                .AddParameter(new OAuth2ClientId(expiredCredentials.ClientId))
                .AddParameter(new OAuth2ClientSecret(expiredCredentials.ClientSecret))
                .AddParameter(new OAuth2RefreshToken(expiredCredentials.RefreshToken));

            var token = await GetAccessToken(
                    expiredCredentials.ClientId, 
                    expiredCredentials.ClientSecret,
                    builder)
                .ConfigureAwait(false);

            return token
                .SetTokenName(expiredCredentials.TokenName)
                .TransferRefreshToken(expiredCredentials.RefreshToken);
        }

        private async Task<OAuth2Credentials> GetAccessToken(
            string clientId,
            string clientSecret,
            AuthenticatorBuilder builder)
        {
            builder.AddParameter(new OAuth2GrantType(_resourceProvider.Grant));

            var token = await _oauth.GetToken<OAuth2Credentials>(
                    _resourceProvider.TokenUrl,
                    builder,
                    _resourceProvider.Headers,
                    HttpParameterType.Unspecified)
                .ConfigureAwait(false);

            return token.SetClientProperties(
                    clientId, 
                    clientSecret)
                .TimestampToken();
        }
    }
}
