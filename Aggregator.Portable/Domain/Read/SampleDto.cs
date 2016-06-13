using System;
using Newtonsoft.Json.Linq;
using SimpleCQRS.Domain;

namespace Aggregator.Domain.Write.Samples
{
    public class SampleDto<TRequest> : SampleDto
        where TRequest : Request
    {
    }


    public class SampleDto : Entity
    {
        public SampleDto() { }

        public SampleDto(
            DateTimeOffset timestamp,
            JObject payload,
            Guid id)
        {
            Timestamp = timestamp;
            Payload = payload;
            Id = id;
        }

        public Guid AggregateId { get; set; }
        public DateTimeOffset Timestamp { get; set; }
        public JObject Payload { get; set; }
    }
}

