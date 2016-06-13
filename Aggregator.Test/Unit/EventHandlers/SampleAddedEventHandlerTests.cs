using System;
using System.Linq;
using BatmansBelt;
using Newtonsoft.Json.Linq;
using Aggregator.Domain.Events;
using Aggregator.Domain.Write.Samples;
using Aggregator.Infrastructure.Requests;
using Aggregator.Task.EventHandlers;
using Aggregator.Test.Helpers.Mocks;
using Xunit;

namespace Aggregator.Test.Unit.EventHandlers
{
    public class SampleAddedEventHandlerTests
    {
        [Fact]
        public void HandleSampleAddedEventThrown()
        {
            var databaseMock = new DatabaseMock<SampleDto<FacebookFriend>>();
            var payload = new JObject();
            var timestamp = DateTimeOffset.Now;
            var aggregateId = Guid.NewGuid();
            var @event = new SampleAdded<FacebookFriend>(
                payload, 
                timestamp, 
                Hashing.CreateGuidFromData(payload.ToString()));
            @event.AggregateId = aggregateId;

            var handler = new SampleAddedEventHandler<FacebookFriend>(databaseMock);

            handler.Handle(@event);

            var samples = databaseMock.GetAll(aggregateId.ToString());
            Assert.Equal(1, samples.Count());
            var sample = samples.First();
            Assert.Equal(payload, sample.Payload);
            Assert.Equal(timestamp, sample.Timestamp);
            Assert.Equal(aggregateId, sample.AggregateId);
        }
    }
}
