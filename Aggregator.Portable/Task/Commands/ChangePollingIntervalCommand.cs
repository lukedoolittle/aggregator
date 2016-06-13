using System;
using Aggregator.Domain.Write;
using Aggregator.Framework.Enums;
using SimpleCQRS.Infrastructure;

namespace Aggregator.Task.Commands
{
    public class ChangePollingIntervalCommand<TRequest> : Command
        where TRequest : Request, new()
    {
        public PollingIntervalEnum NewPollingInterval { get; private set; }

        public ChangePollingIntervalCommand(
            Guid aggregateId,
            PollingIntervalEnum newPollingInterval, 
            int originalVersion) : 
            base(aggregateId,
            originalVersion)
        {
            NewPollingInterval = newPollingInterval;
        }
    }
}
