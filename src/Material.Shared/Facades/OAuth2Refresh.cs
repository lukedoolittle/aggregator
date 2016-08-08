using System.Threading.Tasks;
using Material.Infrastructure.Credentials;
using Material.Infrastructure.Task;
using Material.OAuth;

namespace Material.Infrastructure.OAuth
{
    public class OAuth2Refresh<TResourceProvider>
        where TResourceProvider : OAuth2ResourceProvider, new()
    {
        private readonly TResourceProvider _provider;

        public OAuth2Refresh(TResourceProvider provider = null)
        {
            _provider = provider ?? new TResourceProvider();
        }

        public Task<OAuth2Credentials> RefreshCredentialsAsync(
            OAuth2Credentials expiredToken)
        {
            var factory = new OAuthFactory();

            return new RefreshTokenFacade(factory.GetOAuth2())
                .GetRefreshedAccessTokenCredentials(
                    expiredToken,
                    _provider);
        }
    }
}
