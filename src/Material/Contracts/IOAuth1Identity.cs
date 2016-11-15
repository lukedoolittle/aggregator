using System.Threading.Tasks;
using Material.Infrastructure.Credentials;

namespace Material.Contracts
{
    public interface IOAuth1Identity
    {
        Task<JsonWebToken> AppendIdentity(
            JsonWebToken token,
            OAuth1Credentials credentials);
    }
}
