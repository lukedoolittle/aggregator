using System;
using Newtonsoft.Json.Linq;
using Aggregator.Domain.Write;
using SimpleCQRS.Domain;

namespace Aggregator.Domain.Events
{
    public class SampleAdded<TRequest> : Event
        where TRequest : Request
    {
        public JObject Payload { get; private set; }
        public DateTimeOffset Timestamp { get; private set; }
        public Guid PayloadHash { get; private set; }

        public SampleAdded(
            JObject payload, 
            DateTimeOffset timestamp,
            Guid payloadHash)
        {
            Payload = payload;
            Timestamp = timestamp;
            PayloadHash = payloadHash;
        }
    }
}
