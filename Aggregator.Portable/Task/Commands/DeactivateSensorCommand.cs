using System;
using Aggregator.Domain.Write;
using SimpleCQRS.Infrastructure;

namespace Aggregator.Task.Commands
{
    public class DeactivateSensorCommand<TRequest> : Command
        where TRequest : Request, new()
    {
        public DeactivateSensorCommand(
            Guid aggregateId,
            int originalVersion) : 
            base(aggregateId,
            originalVersion)
        {
        }
    }
}
