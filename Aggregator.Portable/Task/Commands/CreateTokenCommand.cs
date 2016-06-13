using System;
using Newtonsoft.Json.Linq;
using Aggregator.Domain.Write;
using SimpleCQRS.Infrastructure;

namespace Aggregator.Task.Commands
{
    public class CreateTokenCommand<TService> : Command
        where TService: Service
    {
        public JObject Values { get; private set; }

        public CreateTokenCommand(
            Guid aggregateId,
            JObject values,
            int originalVersion) : 
            base(
                aggregateId,
                originalVersion)
        {
            Values = values;
        }
    }
}
