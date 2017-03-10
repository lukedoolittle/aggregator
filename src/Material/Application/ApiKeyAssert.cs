using System.Threading.Tasks;
using Material.Infrastructure;
using Material.Infrastructure.Credentials;
using Material.OAuth.Authorization;

namespace Material.Application
{
    public class ApiKeyAssert<TResourceProvider>
        where TResourceProvider : ApiKeyExchangeResourceProvider, new()
    {
        private readonly string _apiKey;

        public ApiKeyAssert(string apiKey)
        {
            _apiKey = apiKey;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")]
        public Task<OAuth2Credentials> GetCredentialsAsync()
        {
            var provider = new TResourceProvider();

            return new ApiKeyJsonWebTokenExchangeAuthorizationAdapter()
                .GetAccessToken(
                    provider.TokenUrl, 
                    provider.KeyName, 
                    _apiKey, 
                    provider.KeyType, 
                    provider.TokenName,
                    provider.OpenIdDiscoveryUrl);
        }
    }
}
