using System.Threading.Tasks;
using Material.Domain.Credentials;

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
