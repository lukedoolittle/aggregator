using Material.Infrastructure;
using Material.Infrastructure.Credentials;

namespace Material.Contracts
{
    public interface IClientCredentials
    {
        TCredentials GetClientCredentials<TService, TCredentials>()
            where TService : ResourceProvider
            where TCredentials : TokenCredentials;
    }
}
