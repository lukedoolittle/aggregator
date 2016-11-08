using System.Threading.Tasks;
using Material.Infrastructure.Credentials;

namespace Material.Contracts
{
    public interface IOAuthAuthorizationTemplate<TCredentials>
        where TCredentials : TokenCredentials
    {
        Task<TCredentials> GetAccessTokenCredentials(string userId);
    }
}
