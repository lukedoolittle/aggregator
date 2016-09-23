using System.Threading.Tasks;
using Material.Infrastructure.Credentials;

namespace Material.Infrastructure.OAuth
{
    /// <summary>
    /// Authenticates a resource owner with the given resource provider using OAuth2
    /// </summary>
    /// <typeparam name="TResourceProvider">Resource provider to authenticate with</typeparam>
    public class OAuth2Client<TResourceProvider>
        where TResourceProvider : OAuth2ResourceProvider, new()
    {
        private readonly string _clientId;
        private readonly string _clientSecret;

        public OAuth2Client(
            string clientId, 
            string clientSecret)
        {
            _clientId = clientId;
            _clientSecret = clientSecret;
        }

        /// <summary>
        /// Authenticates a resource owner using the OAuth2 Client Credentials workflow
        /// </summary>
        /// <returns>OAuth2Credentials with access token</returns>
        public Task<OAuth2Credentials> GetCredentialsAsync()
        {
            return new OAuthClientFacade(
                    new OAuth2Authentication())
                .GetClientAccessTokenCredentials<TResourceProvider>(
                    _clientId, 
                    _clientSecret);
        }
    }
}
