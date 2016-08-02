using System.Threading.Tasks;
using Material.Infrastructure.Credentials;

namespace Material.Contracts
{
    public interface IOAuthAuthenticationTemplate<TCredentials>
        where TCredentials : TokenCredentials
    {
        Task<TCredentials> GetAccessTokenCredentials(string userId);
    }
}
