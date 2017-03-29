using System.Threading.Tasks;
using Material.Authorization;
using Material.Domain.Core;
using Material.Domain.Credentials;
using Material.Workflow.Facade;

namespace Material.Application
{
    /// <summary>
    /// Authenticates a resource owner with the given resource provider using OAuth2
    /// </summary>
    /// <typeparam name="TResourceProvider">Resource provider to authenticate with</typeparam>
    public class OAuth2Refresh<TResourceProvider>
        where TResourceProvider : OAuth2ResourceProvider, new()
    {
        private readonly OAuthClientFacade<TResourceProvider> _facade;

        public OAuth2Refresh() : 
            this(new TResourceProvider())
        { }

        public OAuth2Refresh(TResourceProvider resourceProvider)
        {
            _facade = new OAuthClientFacade<TResourceProvider>(
                new OAuthAuthorizationAdapter(),
                resourceProvider);
        }

        /// <summary>
        /// Re-authenticates a user using the OAuth2 Refresh Token workflow
        /// </summary>
        /// <param name="expiredToken">Token with valid refresh token and expired access token</param>
        /// <returns>OAuth2Credentials with non expired access token</returns>
        public Task<OAuth2Credentials> RefreshCredentialsAsync(
            OAuth2Credentials expiredToken)
        {
            return _facade
                .GetRefreshedAccessTokenCredentials(
                    expiredToken);
        }
    }
}