using System;
using Aggregator.Domain.Write;
using Aggregator.Framework.Enums;
using SimpleCQRS.Infrastructure;

namespace Aggregator.Task.Commands
{
    public class CreateSensorCommand<TRequest> : Command
        where TRequest : Request, new()
    {
        public PollingIntervalEnum PollingInterval { get; private set; }
        public bool RequiresAuthentication { get; private set; }

        public CreateSensorCommand(
            Guid aggregateId,
            int originalVersion,
            PollingIntervalEnum pollingInterval = PollingIntervalEnum.Moderate,
            bool requiresAuthentication = true) : 
            base(
                aggregateId,
                originalVersion)
        {
            PollingInterval = pollingInterval;
            RequiresAuthentication = requiresAuthentication;
        }
    }
}
