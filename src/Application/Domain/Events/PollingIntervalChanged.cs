using System;
using Aggregator.Domain.Write;
using Material.Infrastructure;
using SimpleCQRS.Domain;

namespace Aggregator.Domain.Events
{
    public class PollingIntervalChanged<TRequest> : Event
        where TRequest : Request, new()
    {
        public TimeSpan NewPollingInterval { get; private set; }

        public PollingIntervalChanged(TimeSpan newPollingInterval)
        {
            NewPollingInterval = newPollingInterval;
        }
    }
}
