using Aggregator.Domain.Write;
using Aggregator.Framework.Enums;
using SimpleCQRS.Domain;

namespace Aggregator.Domain.Events
{
    public class PollingIntervalChanged<TRequest> : Event
        where TRequest : Request, new()
    {
        public PollingIntervalEnum NewPollingInterval { get; private set; }

        public PollingIntervalChanged(PollingIntervalEnum newPollingInterval)
        {
            NewPollingInterval = newPollingInterval;
        }
    }
}
