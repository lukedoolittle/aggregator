using System.Threading.Tasks;
using Material.Infrastructure.Credentials;
using Material.OAuth;

namespace Material.Infrastructure.OAuth
{
    public class OAuth2RefreshFacade<TResourceProvider>
        where TResourceProvider : OAuth2ResourceProvider, new()
    {
        private readonly TResourceProvider _provider;

        public OAuth2RefreshFacade(TResourceProvider provider = null)
        {
            _provider = provider ?? new TResourceProvider();
        }

        public Task<OAuth2Credentials> RefreshOAuth2Credentials(
            OAuth2Credentials expiredToken)
        {
            return new RefreshTokenFacade(new OAuth2Authentication())
                .GetRefreshedAccessTokenCredentials(
                    expiredToken,
                    _provider);
        }
    }
}
