using System;
using Aggregator.Infrastructure;
using Material.Infrastructure;

namespace Aggregator.Framework.Contracts
{
    public interface IAuthenticationRefreshTaskFactory
    {
        ITask GenerateRefreshTask<TService>(Guid aggregateId) 
            where TService : OAuth2ResourceProvider, new();
    }
}
