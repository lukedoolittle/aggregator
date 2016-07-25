using System;
using Aggregator.Domain.Write;
using Material.Infrastructure;
using SimpleCQRS.Infrastructure;

namespace Aggregator.Task.Commands
{
    public class ChangePollingIntervalCommand<TRequest> : Command
        where TRequest : Request, new()
    {
        public TimeSpan NewPollingInterval { get; private set; }

        public ChangePollingIntervalCommand(
            Guid aggregateId,
            TimeSpan newPollingInterval, 
            int originalVersion) : 
            base(aggregateId,
            originalVersion)
        {
            NewPollingInterval = newPollingInterval;
        }
    }
}
