using System.Threading.Tasks;
using Material.Infrastructure.Credentials;

namespace Material.Contracts
{
    public interface IOAuthAccessTokenFacade<TCredentials>
        where TCredentials : TokenCredentials
    {
        /// <summary>
        /// Exchanges intermediate credentials for access token credentials
        /// </summary>
        /// <param name="intermediateResult">Intermediate credentials received from OAuth2 callback</param>
        /// <param name="userId">Resource owner's Id</param>
        /// <returns>Access token credentials</returns>
        Task<TCredentials> GetAccessTokenAsync(
            TCredentials intermediateResult,
            string userId);
    }
}
