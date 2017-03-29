using System.Threading.Tasks;
using Material.Domain.Credentials;

namespace Material.Contracts
{
    public interface IOAuthAuthorizationTemplate<TCredentials>
        where TCredentials : TokenCredentials
    {
        Task<TCredentials> GetAccessTokenCredentials(string requestId);
    }
}
