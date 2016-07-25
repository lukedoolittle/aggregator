using System;
using Newtonsoft.Json.Linq;
using Aggregator.Domain.Write;
using Material.Infrastructure;
using SimpleCQRS.Infrastructure;

namespace Aggregator.Task.Commands
{
    public class UpdateTokenCommand<TService> : Command
        where TService : ResourceProvider
    {
        public JObject NewValues { get; private set; }

        public UpdateTokenCommand(
            Guid aggregateId,
            JObject newValues, 
            int originalVersion) : 
            base(
            aggregateId,
            originalVersion)
        {
            NewValues = newValues;
        }
    }
}
