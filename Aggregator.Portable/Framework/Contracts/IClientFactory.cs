using System;
using Aggregator.Domain.Write;

namespace Aggregator.Framework.Contracts
{
    public interface IClientFactory
    {
        IRequestClient CreateClient<TRequest>(object credentials)
            where TRequest : Request;

        IRequestClient CreateClient(Type requestType, object credentials);

        IRequestClient CreateClient(Type requestType);
    }
}
