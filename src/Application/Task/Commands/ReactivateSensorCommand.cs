using System;
using Aggregator.Domain.Write;
using Material.Infrastructure;
using SimpleCQRS.Infrastructure;

namespace Aggregator.Task.Commands
{
    public class ReactivateSensorCommand<TRequest> : Command
        where TRequest : Request, new()
    {
        public ReactivateSensorCommand(
            Guid aggregateId,
            int originalVersion) : 
            base(
            aggregateId,
            originalVersion)
        {
        }
    }
}
