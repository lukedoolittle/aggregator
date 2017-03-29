using System.Threading.Tasks;
using Material.Domain.Credentials;

namespace Material.Contracts
{
    public interface IOAuthAccessTokenFacade<TCredentials>
        where TCredentials : TokenCredentials
    {
        /// <summary>
        /// Exchanges intermediate credentials for access token credentials
        /// </summary>
        /// <param name="intermediateResult">Intermediate credentials received from OAuth2 callback</param>
        /// <param name="requestId">Unique ID of the request being made</param>
        /// <returns>Access token credentials</returns>
        Task<TCredentials> GetAccessTokenAsync(
            TCredentials intermediateResult,
            string requestId);
    }
}
