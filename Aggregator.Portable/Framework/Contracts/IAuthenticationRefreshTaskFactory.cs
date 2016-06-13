using System;

namespace Aggregator.Framework.Contracts
{
    public interface IAuthenticationRefreshTaskFactory
    {
        ITask GenerateRefreshTask(
            Type serviceType, 
            Guid aggregateId);
    }
}
