using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using Aggregator.Domain.Write;
using SimpleCQRS.Infrastructure;

namespace Aggregator.Task.Commands
{
    public class ChangeFilterCommand<TRequest> : Command
        where TRequest : Request, new()
    {
        public IEnumerable<Tuple<DateTimeOffset, JObject>> Samples { get; private set; }

        public ChangeFilterCommand(
            Guid aggregateId,
            int originalVersion,
            IEnumerable<Tuple<DateTimeOffset, JObject>> samples) : 
            base(
                aggregateId,
                originalVersion)
        {
            Samples = samples;
        }
    }
}
