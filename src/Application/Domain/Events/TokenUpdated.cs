using System;
using Newtonsoft.Json.Linq;
using Aggregator.Domain.Write;
using Material.Infrastructure;
using SimpleCQRS.Domain;

namespace Aggregator.Domain.Events
{
    public class TokenUpdated<TService> : Event
        where TService : ResourceProvider
    {
        public JObject Values { get; private set; }

        public string ServiceName => typeof (TService).Name;

        public TokenUpdated(JObject values)
        {
            Values = values;
        }

        public TokenUpdated(
            JObject values, 
            Guid id,
            Guid aggregateId) : 
            this(values)
        {
            Id = id;
            AggregateId = aggregateId;
        } 
    }
}
