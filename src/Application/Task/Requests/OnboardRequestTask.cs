using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Aggregator.Domain.Write;
using Aggregator.Framework.Contracts;
using Material.Contracts;
using Material.Infrastructure;
using SimpleCQRS.Framework.Contracts;

namespace Aggregator.Task.Requests
{
    public class OnboardRequestTask<TRequest> : 
        SampleRequestTaskBase<TRequest>
        where TRequest : Request, new()
    {
        private readonly IRequestClient _client;

        public OnboardRequestTask(
            IEventPublisher publisher,
            ICommandSender sender, 
            Guid aggregateId, 
            string lastQuery, 
            IRequestClient client) : 
            base(
                publisher,
                sender,
                aggregateId,
                lastQuery)
        {
            _client = client;
        }

        protected override Task<IEnumerable<Tuple<DateTimeOffset, JObject>>> GetSamples()
        {
            return _client.GetDataPoints(_lastQuery);
        }
    }
}