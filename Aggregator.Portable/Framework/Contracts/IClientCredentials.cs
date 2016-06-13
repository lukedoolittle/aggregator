using Aggregator.Domain.Write;
using Aggregator.Infrastructure.Credentials;

namespace Aggregator.Framework.Contracts
{
    public interface IClientCredentials
    {
        TCredentials GetClientCredentials<TService, TCredentials>()
            where TService : Service
            where TCredentials : TokenCredentials;
    }
}
