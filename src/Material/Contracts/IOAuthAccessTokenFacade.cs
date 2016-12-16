using System.Threading.Tasks;
using Material.Infrastructure.Credentials;

namespace Material.Contracts
{
    public interface IOAuthAccessTokenFacade<TCredentials>
        where TCredentials : TokenCredentials
    {
        Task<TCredentials> GetAccessTokenAsync(
            TCredentials intermediateResult,
            string userId);
    }
}
