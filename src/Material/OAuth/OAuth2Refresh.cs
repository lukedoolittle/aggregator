using System.Threading.Tasks;
using Material.Infrastructure.Credentials;

namespace Material.Infrastructure.OAuth
{
    /// <summary>
    /// Authenticates a resource owner with the given resource provider using OAuth2
    /// </summary>
    /// <typeparam name="TResourceProvider">Resource provider to authenticate with</typeparam>
    public class OAuth2Refresh<TResourceProvider>
        where TResourceProvider : OAuth2ResourceProvider, new()
    {
        private readonly TResourceProvider _provider;

        public OAuth2Refresh(TResourceProvider provider = null)
        {
            _provider = provider ?? new TResourceProvider();
        }

        /// <summary>
        /// Re-authenticates a user using the OAuth2 Refresh Token workflow
        /// </summary>
        /// <param name="expiredToken">Token with valid refresh token and expired access token</param>
        /// <returns>OAuth2Credentials with non expired access token</returns>
        public Task<OAuth2Credentials> RefreshCredentialsAsync(
            OAuth2Credentials expiredToken)
        {
            return new OAuthClientFacade(
                    new OAuth2Authentication())
                .GetRefreshedAccessTokenCredentials(
                    expiredToken,
                    _provider);
        }
    }
}
