using System.Threading.Tasks;
using Material.Infrastructure.Credentials;

namespace Material.Contracts
{
    public interface IOAuthIdentity<TCredentials>
        where TCredentials : TokenCredentials
    {
        Task<JsonWebToken> AppendIdentity(
            JsonWebToken token,
            TCredentials credentials);
    }
}
