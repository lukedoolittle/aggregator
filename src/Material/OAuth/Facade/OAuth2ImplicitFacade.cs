using System.Threading.Tasks;
using Material.Contracts;
using Material.Infrastructure.Credentials;

namespace Material.OAuth.Facade
{
    public class OAuth2ImplicitFacade : IOAuthAccessTokenFacade<OAuth2Credentials>
    {
        /// <summary>
        /// Returns intermediate result, which contains the access token
        /// </summary>
        /// <param name="intermediateResult">Intermediate credentials received from OAuth2 callback</param>
        /// <param name="userId">Resource owner's Id</param>
        /// <returns>Access token credentials</returns>
        public Task<OAuth2Credentials> GetAccessTokenAsync(
            OAuth2Credentials intermediateResult, 
            string userId)
        {
            return Task.FromResult(intermediateResult);
        }
    }
}
