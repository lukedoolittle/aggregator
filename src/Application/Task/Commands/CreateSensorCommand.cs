using System;
using Aggregator.Domain.Write;
using Material.Infrastructure;
using SimpleCQRS.Infrastructure;

namespace Aggregator.Task.Commands
{
    public class CreateSensorCommand<TRequest> : Command
        where TRequest : Request, new()
    {
        public TimeSpan PollingInterval { get; private set; }
        public bool RequiresAuthentication { get; private set; }

        public CreateSensorCommand(
            Guid aggregateId,
            int originalVersion,
            TimeSpan pollingInterval = default(TimeSpan),
            bool requiresAuthentication = true) : 
            base(
                aggregateId,
                originalVersion)
        {
            if (pollingInterval == default(TimeSpan))
            {
                pollingInterval = TimeSpan.FromHours(1);
            }
            PollingInterval = pollingInterval;
            RequiresAuthentication = requiresAuthentication;
        }
    }
}
