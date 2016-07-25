using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;
using Aggregator.Domain.Events;
using Aggregator.Domain.Write;
using Aggregator.Framework.Exceptions;
using Aggregator.Test.Helpers;
using Material.Infrastructure.ProtectedResources;
using Material.Infrastructure.Requests;
using SimpleCQRS.Domain;
using Xunit;

namespace Aggregator.Test.Unit.Domain
{
    public class PersonTests
    {

        [Fact]
        public void CreatingAPersonWithDefaultConstructorDoesNotThrowException()
        {
            var root = new Person();

            var events = root.GetUncommittedChanges();

            Assert.Equal(0, events.Count());
        }

        [Fact]
        public void CreatingAPersonWithAGuidRegistersAnEvent()
        {
            var root = new Person(Guid.NewGuid());

            var events = root.GetUncommittedChanges();

            Assert.Equal(1, events.Count());
            Assert.True(events.First() is PersonCreated);
        }

        [Fact]
        public void CreatingAPersonAndThenClearingChangesRemovesAllUncomittedChanges()
        {
            var root = new Person(Guid.NewGuid());

            var events = root.GetUncommittedChanges();

            Assert.Equal(1, events.Count());
            
            root.MarkChangesAsCommitted();

            Assert.Equal(0, events.Count());
        }

        [Fact]
        public void CreatingATokenAndThenASensorRegistersEventsAndCreatesDomainObjects()
        {
            var root = new Person(Guid.NewGuid());
            root.MarkChangesAsCommitted();

            root.CreateToken<Twitter>(new JObject());
            root.AddSensor<TwitterFavorite>(TimeSpan.FromSeconds(1), true);

            var changes = root.GetUncommittedChanges();

            var enumerable = changes as Event[] ?? changes.ToArray();
            Assert.Equal(2, enumerable.Count());
            Assert.True(enumerable[0] is TokenCreated<Twitter>);
            Assert.True(enumerable[1] is SensorCreated<TwitterFavorite>);

            var sensors = root.GetMemberValue<ICollection<Sensor>>("_sensors");

            Assert.Equal(1, sensors.Count);
        }

        [Fact]
        public void CreatingTwoTokensForTheSameServiceThrowsAnException()
        {
            var root = new Person(Guid.NewGuid());

            root.CreateToken<Twitter>(new JObject());
            Assert.Throws<DuplicateTokenException>(()=>root.CreateToken<Twitter>(new JObject()));
        }

        [Fact]
        public void CreatingSensorWithAnUnauthenticatedServiceThrowsAnException()
        {
            var root = new Person(Guid.NewGuid());
            Assert.Throws<ServiceAuthenticationTokenNotFoundException>(()=>root.AddSensor<TwitterFavorite>(TimeSpan.FromSeconds(1), true));
        }

        [Fact]
        public void CreatingAnUnauthenticatedSensorWithAnUnAuthenticatedServiceDoesntThrowAnException()
        {
            var root = new Person(Guid.NewGuid());
            root.AddSensor<TwitterFavorite>(TimeSpan.FromSeconds(1), false);
        }

        [Fact]
        public void UpdatingATokenThatDoesNotExistThrowsAnException()
        {
            var root = new Person(Guid.NewGuid());
            Assert.Throws<ServiceAuthenticationTokenNotFoundException>(() => root.UpdateToken<Twitter>(new JObject()));
        }

        [Fact]
        public void UpdatingATokenChangesItsInternalValues()
        {
            var oldToken = new JObject();
            var expected = new JObject();

            var root = new Person(Guid.NewGuid());
            root.CreateToken<Twitter>(oldToken);
            root.UpdateToken<Twitter>(expected);

            var tokens = root.GetMemberValue<ICollection<Token>>("_tokens");

            Assert.Equal(1, tokens.Count);

            var token = tokens.Single(t => t is Token<Twitter>);

            Assert.Equal(expected, token.Values);
        }

        [Fact]
        public void ChangingAPollingIntervalOnASensorThatDoesNotExistThrowsAnException()
        {
            var root = new Person(Guid.NewGuid());
            Assert.Throws<SensorNotFoundException>(() => root.ChangePollingInterval<TwitterRetweetOfMe>(TimeSpan.FromSeconds(1)));
        }

        [Fact]
        public void ChangingPollingIntervalForASensorUpdatesThatSensorsValues()
        {
            var oldPollingInterval = TimeSpan.FromSeconds(1);
            var expected = TimeSpan.FromSeconds(1);

            var root = new Person(Guid.NewGuid());
            root.CreateToken<Twitter>(new JObject());
            root.AddSensor<TwitterFavorite>(oldPollingInterval, true);
            root.ChangePollingInterval<TwitterFavorite>(expected);

            var sensors = root.GetMemberValue<ICollection<Sensor>>("_sensors");

            Assert.Equal(1, sensors.Count);

            var sensor = sensors.Single(s => s is Sensor<TwitterFavorite>);

            var actual = sensor.GetMemberValue<TimeSpan>("_pollingInterval");

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void DeactivatingASensorMarksThatSensorDeactivated()
        {
            var expected = false;
            var root = new Person(Guid.NewGuid());
            root.CreateToken<Twitter>(new JObject());
            root.AddSensor<TwitterFavorite>(TimeSpan.FromSeconds(1), true);
            root.DeactivateSensor<TwitterFavorite>();

            var sensors = root.GetMemberValue<ICollection<Sensor>>("_sensors");

            Assert.Equal(1, sensors.Count);

            var sensor = sensors.Single(s => s is Sensor<TwitterFavorite>);

            var actual = sensor.GetMemberValue<bool>("_isActive");

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ActivatingASensorMarksThatSensorActive()
        {
            var expected = true;
            var root = new Person(Guid.NewGuid());
            root.CreateToken<Twitter>(new JObject());
            root.AddSensor<TwitterFavorite>(TimeSpan.FromSeconds(1), true);
            root.DeactivateSensor<TwitterFavorite>();
            root.ReactivateSensor<TwitterFavorite>();

            var sensors = root.GetMemberValue<ICollection<Sensor>>("_sensors");

            Assert.Equal(1, sensors.Count);

            var sensor = sensors.Single(s => s is Sensor<TwitterFavorite>);

            var actual = sensor.GetMemberValue<bool>("_isActive");

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void DeactivatingASensorThatDoesNotExistThrowsAnException()
        {
            var root = new Person(Guid.NewGuid());
            root.MarkChangesAsCommitted();

            Assert.Throws<SensorNotFoundException>(() => root.DeactivateSensor<TwitterRetweetOfMe>());
        }

        [Fact]
        public void ActivatingASensorThatDoesNotExistThrowsAnException()
        {
            var root = new Person(Guid.NewGuid());
            root.MarkChangesAsCommitted();

            Assert.Throws<SensorNotFoundException>(() => root.ReactivateSensor<TwitterRetweetOfMe>());
        }

        [Fact]
        public void ActivatingAnActiveSensorThrowsAnException()
        {
            var root = new Person(Guid.NewGuid());
            root.MarkChangesAsCommitted();

            root.CreateToken<Twitter>(new JObject());
            root.AddSensor<TwitterFavorite>(TimeSpan.FromSeconds(1), true);
            root.ReactivateSensor<TwitterFavorite>();
        }

        [Fact]
        public void DeactivatingAnInactiveSensorThrowsAnException()
        {
            var root = new Person(Guid.NewGuid());
            root.MarkChangesAsCommitted();

            root.CreateToken<Twitter>(new JObject());
            root.AddSensor<TwitterFavorite>(TimeSpan.FromSeconds(1), true);
            root.DeactivateSensor<TwitterFavorite>();
            root.DeactivateSensor<TwitterFavorite>();
        }

        [Fact]
        public void AddingSamplesWithoutAMatchingFilterPropertyDoesntAddAnyEvents()
        {
            var expectedCount = 0;
            var sampleCount = 5;
            var samples = CreateSomeSamples(sampleCount);

            var root = new Person(Guid.NewGuid());
            root.CreateToken<Facebook>(new JObject());
            root.AddSensor<FacebookEvent>(TimeSpan.FromSeconds(1), true);
            root.MarkChangesAsCommitted();

            root.ChangeFilter<FacebookEvent>(samples);

            Assert.Equal(expectedCount, root.GetUncommittedChanges().Count());
        }

        [Fact]
        public void AddingSamplesWithMatchingFilterPropertyCreatesAnEvent()
        {
            var expectedCount = 0;
            var sampleCount = 5;
            var samples = CreateSomeSamples(
                sampleCount, 
                new FacebookEvent().ResponseTimestamp.TimestampFormat,
                new FacebookEvent().ResponseFilterKey);

            var root = new Person(Guid.NewGuid());
            root.CreateToken<Facebook>(new JObject());
            root.AddSensor<FacebookEvent>(TimeSpan.FromSeconds(1), true);
            root.MarkChangesAsCommitted();

            root.ChangeFilter<FacebookEvent>(samples);

            Assert.Equal(1 + expectedCount, root.GetUncommittedChanges().Count());

            var queuedEvent = root.GetUncommittedChanges().FirstOrDefault(e => e is FilterChanged<FacebookEvent>);
            Assert.NotNull(queuedEvent);
        }

        private IEnumerable<Tuple<DateTimeOffset, JObject>> CreateSomeSamples(
            int count,
            string dateFormat = "",
            string filterProperty = "")
        {
            var samples = new List<Tuple<DateTimeOffset, JObject>>();
            var baseDateTimeOffset = DateTimeOffset.Now;

            for (var i = 0; i < count; i++)
            {
                var payload = new JObject {["someProperty"] = Guid.NewGuid() };
                var sample = new Tuple<DateTimeOffset, JObject>(baseDateTimeOffset.AddSeconds(i), payload);
                samples.Add(sample);

                if (filterProperty != string.Empty)
                {
                    payload[filterProperty] = DateTime.Now.ToString(dateFormat);
                }
            }

            return samples;
        }
    }
}
