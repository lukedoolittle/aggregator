using System.Threading.Tasks;
using Material.Infrastructure;
using Material.Infrastructure.Credentials;
using Material.Infrastructure.OAuth.Authorization;

namespace Material.Infrastructure.OAuth
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

            return new ApiKeyJsonWebTokenExchangeAdapter()
                .GetAccessToken(
                    provider.TokenUrl, 
                    provider.KeyName, 
                    _apiKey, 
                    provider.KeyType, 
                    provider.TokenName);
        }
    }
}
