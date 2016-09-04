using System.Threading.Tasks;
using Material.Infrastructure.Credentials;

namespace Material.Infrastructure.OAuth
{
    public class OAuth2Client<TResourceProvider>
        where TResourceProvider : OAuth2ResourceProvider, new()
    {
        public Task<OAuth2Credentials> GetCredentialsAsync(
            string clientId,
            string clientSecret)
        {
            return new OAuthClientFacade(
                    new OAuth2Authentication())
                .GetClientAccessTokenCredentials<TResourceProvider>(
                    clientId, 
                    clientSecret);
        }
    }
}
