using System;
using Newtonsoft.Json.Linq;
using Aggregator.Domain.Events;
using Aggregator.Domain.Read;
using Aggregator.Task.EventHandlers;
using Aggregator.Test.Helpers.Mocks;
using Material.Infrastructure.ProtectedResources;
using Material.Infrastructure.Requests;
using Xunit;
using TwitterTweet = Material.Infrastructure.Requests.TwitterTweet;

namespace Aggregator.Test.Unit.EventHandlers
{
    class TokenUpdatedEventHandlerFixture
    {
        [Fact]
        public void HandleTokenUpdatedEventThrow()
        {
            var aggregateId = Guid.NewGuid();
            var oldToken = new JObject();

            var sensorOne = new SensorDto(
                Guid.NewGuid(), 
                aggregateId, 
                TimeSpan.FromSeconds(1), 
                oldToken, 
                typeof(Facebook), 
                typeof(FacebookEvent), 
                null,
                0);
            var sensorTwo = new SensorDto(
                Guid.NewGuid(),
                aggregateId,
                TimeSpan.FromSeconds(1),
                oldToken,
                typeof(Facebook),
                typeof(FacebookEvent),
                null,
                0);
            var sensorThree = new SensorDto(
                Guid.NewGuid(),
                aggregateId,
                TimeSpan.FromSeconds(1),
                oldToken,
                typeof(Twitter),
                typeof(TwitterTweet),
                null,
                0);

            var databaseMock = new DatabaseMock<SensorDto>();
            databaseMock.Put(sensorOne);
            databaseMock.Put(sensorTwo);
            databaseMock.Put(sensorThree);

            var expected = new JObject();
            var @event = new TokenUpdated<Facebook>(expected) {AggregateId = aggregateId};

            var eventHandler = new TokenUpdatedEventHandler<Facebook>(databaseMock);

            eventHandler.Handle(@event);

            Assert.Equal(expected, sensorOne.Token);
            Assert.Equal(expected, sensorTwo.Token);
            Assert.Equal(oldToken, sensorThree.Token);
        }
    }
}
