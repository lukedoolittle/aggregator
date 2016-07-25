using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Newtonsoft.Json.Linq;
using Aggregator.Domain.Events;
using Aggregator.Framework.Contracts;
using Aggregator.Framework.Exceptions;
using Aggregator.Framework.Extensions;
using Foundations.Extensions;
using Material.Contracts;
using Material.Infrastructure;
using Material.Metadata;
using SimpleCQRS.Domain;

namespace Aggregator.Domain.Write
{
    public sealed class Person : AggregateRoot
    {
        private readonly ICollection<Sensor> _sensors;
        private readonly ICollection<Token> _tokens; 
        private Guid _id;

        public override Guid Id => _id;

        public Person()
        {
            _sensors = new Collection<Sensor>();
            _tokens = new Collection<Token>();

            RegisterConflictResolvers();
        }

        public Person(Guid id) : 
            this()
        {
            ApplyChange(new PersonCreated(id));
        }

        protected override void RegisterConflictResolvers()
        {
            RegisterConflictResolvers(
                typeof(SampleAdded<>), 
                new List<Type>(),
                false);
            RegisterConflictResolvers(
                typeof(TokenUpdated<>),
                new List<Type>(),
                false);

            //Don't put in PersonCreated because it conflicts with everything!

            RegisterConflictResolvers(
                typeof(FilterChanged<>),
                new List<Type>(),
                true);
            RegisterConflictResolvers(
                typeof(PollingIntervalChanged<>),
                new List<Type>(),
                true);
            RegisterConflictResolvers(
                typeof(SensorCreated<>),
                new List<Type>(),
                true);
            RegisterConflictResolvers(
                typeof(SensorDeactivated<>),
                new List<Type> { typeof(SensorReactivated<>) },
                false);
            RegisterConflictResolvers(
                typeof(SensorReactivated<>),
                new List<Type> { typeof(SensorDeactivated<>)},
                false);
            RegisterConflictResolvers(
                typeof(TokenCreated<>),
                new List<Type> {typeof(TokenUpdated<>)},
                true);
        }

        #region Handlers

        private void OnPersonCreated(PersonCreated e)
        {
            _id = e.PersonId;
        }

        private void OnSensorCreated<TSensor>(SensorCreated<TSensor> e)
            where TSensor : Request, new()
        {
            var sensor = new Sensor<TSensor>(
                e.SensorId,
                e.PollingInterval);

            _sensors.Add(sensor);
            _entities.Add(sensor);
        }

        private void OnTokenCreated<TService>(TokenCreated<TService> e)
            where TService : ResourceProvider
        {
            var token = new Token<TService>(
                Guid.NewGuid(), 
                e.Values);

            _tokens.Add(token);
            _entities.Add(token);
        }

        private void OnTokenUpdated<TService>(TokenUpdated<TService> e)
            where TService : ResourceProvider
        {
            var oldToken = _tokens.Single(
                a => a.ServiceName == e.ServiceName);
            var newToken = new Token<TService>(oldToken.Id, e.Values);
            _tokens.Remove(oldToken);
            _entities.Remove(oldToken);
            _tokens.Add(newToken);
            _entities.Add(newToken);
        }

        #endregion Handlers

        #region Domain Methods

        public void AddSensor<TRequest>(
            TimeSpan pollingInterval,
            bool requiresAuthentication)
            where TRequest : Request, new()
        {
            if (requiresAuthentication)
            {
                var serviceToken = GetToken(
                    typeof(TRequest).GetCustomAttributes<ServiceType>().Single().Type);

                if (serviceToken == null)
                {
                    throw new ServiceAuthenticationTokenNotFoundException();
                }

                ApplyChange(
                    new SensorCreated<TRequest>(
                        Guid.NewGuid(),
                        pollingInterval,
                        serviceToken.Values));
            }
            else
            {
                ApplyChange(
                    new SensorCreated<TRequest>(
                        Guid.NewGuid(),
                        pollingInterval,
                        null));
            }
        }

        public void ChangeFilter<TRequest>(IEnumerable<Tuple<DateTimeOffset, JObject>> samples)
            where TRequest : Request, new()
        {
            var request = new TRequest() as IFilterable;

            if (!string.IsNullOrEmpty(request.ResponseFilterKey))
            {
                var newFilter = GetNewFilter(
                    samples,
                    request.ResponseFilterKey,
                    request.ResponseTimestamp);
                if (newFilter != string.Empty)
                {
                    ApplyChange(new FilterChanged<TRequest>(newFilter));
                }
            }
        }

        public void DeactivateSensor<TRequest>()
            where TRequest : Request, new()
        {
            var sensor = GetSensor<TRequest>();

            ApplyChange(new SensorDeactivated<TRequest>(sensor.SensorId));
        }

        public void ReactivateSensor<TRequest>()
            where TRequest : Request, new()
        {
            var sensor = GetSensor<TRequest>();

            ApplyChange(new SensorReactivated<TRequest>(sensor.SensorId));
        }

        public void CreateToken<TService>(JObject values)
            where TService : ResourceProvider
        {
            var serviceToken = GetToken<TService>();

            if (serviceToken != null)
            {
                throw new DuplicateTokenException();
            }

            ApplyChange(new TokenCreated<TService>(values));
        }

        public void UpdateToken<TService>(JObject values)
            where TService : ResourceProvider
        {
            var serviceToken = GetToken<TService>();

            if (serviceToken == null)
            {
                throw new ServiceAuthenticationTokenNotFoundException();
            }

            ApplyChange(new TokenUpdated<TService>(values));
        }

        public void ChangePollingInterval<TRequest>(
            TimeSpan newPollingInterval)
            where TRequest : Request, new()
        {
            GetSensor<TRequest>();

            ApplyChange(new PollingIntervalChanged<TRequest>(
                newPollingInterval));
        }

        #endregion Domain Methods

        private Sensor<TRequest> GetSensor<TRequest>()
            where TRequest : Request, new()
        {
            var sensor = _sensors.SingleOrDefault(
                s => s is Sensor<TRequest>);

            if (sensor == null)
            {
                throw new SensorNotFoundException();
            }

            return (Sensor<TRequest>)sensor;
        }

        private Sensor GetSensor(Type sampleType)
        {
            var sensor = _sensors.SingleOrDefault(
                s => s.SampleType == sampleType);

            if (sensor == null)
            {
                throw new SensorNotFoundException();
            }

            return sensor;
        }

        private Token GetToken(Type tokenType)
        {
            var serviceToken = _tokens.SingleOrDefault(
                token => token.ServiceName == tokenType.Name);

            return serviceToken;
        }

        private Token<TService> GetToken<TService>()
            where TService : ResourceProvider
        {
            var serviceToken = _tokens.SingleOrDefault(
                token => token is Token<TService>);

            return (Token<TService>)serviceToken;
        }

        private string GetNewFilter(
            IEnumerable<Tuple<DateTimeOffset, JObject>> samples,
            string filterProperty,
            TimestampOptions options)
        {
            var allFilters = samples
                .Where(a => !string.IsNullOrEmpty(a.Item2[filterProperty]?.ToString()))
                .Select(a => a.Item2[filterProperty].ToString())
                .ToArray();

            if (allFilters.Length == 0)
            {
                return string.Empty;
            }

            long dummy;
            //if the properties are long ints, simply return the max of the strings
            //otherwise convert the properties to datetimes and return the max of those
            if (long.TryParse(allFilters.First(), out dummy))
            {
                return allFilters.Max();
            }
            else
            {
                var timestamps = new Dictionary<DateTimeOffset, string>();
                foreach (var filter in allFilters)
                {
                    var filerTimestamp = filter.ToDateTimeOffset(options.TimestampFormat, null);
                    if (!timestamps.Keys.Contains(filerTimestamp))
                        timestamps.Add(filerTimestamp, filter);
                }

                var newestTimestamp = timestamps.Keys.Max();

                return timestamps[newestTimestamp];
            }
        }
    }
}
