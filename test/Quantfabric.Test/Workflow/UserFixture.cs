using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;
using Aggregator.Configuration;
using Aggregator.Domain.Read;
using Aggregator.Domain.Write;
using Aggregator.Domain.Write.Samples;
using Aggregator.Framework.Contracts;
using Aggregator.Task.Commands;
using Material.Infrastructure;
using Quantfabric.Test.Material;
using SimpleCQRS.Framework.Contracts;
using SimpleCQRS.Infrastructure;
using Xunit;

namespace Aggregator.Test.Helpers
{
    using System.Threading.Tasks;

    public class UserFixture : IDisposable
    {
        private readonly IDatabase<EventDescriptors> _eventDatabase;
        private readonly IDatabase<SensorDto> _sensorsDatabase;
        private readonly IDatabase<SampleDto> _samplesDatabase;
        private readonly IRepository<Person> _repository;
        private readonly ReadModelFacade _readModel;
        private readonly IAuthenticationRefreshTaskFactory _authenticationTaskFactory;
        private readonly ICommandSender _commandSender;
        private readonly ConcurrentBag<EventDescriptors> _addedEventDescriptors;
        private readonly ConcurrentBag<SensorDto> _addedSensorDto;
        private readonly ConcurrentBag<SampleDto> _addedSample;

        private int _currentVersion;

        private bool _disposeOfUser = true;

        public Guid PersonId { get; private set; }

        public UserFixture(
            IDatabase<EventDescriptors> eventDatabase,
            IDatabase<SensorDto> sensorReadDatabase,
            IDatabase<SampleDto> samplesReadDatabase,
            IRepository<Person> repository,
            ReadModelFacade readModel,
            IAuthenticationRefreshTaskFactory authenticationFactory,
            ICommandSender commandSender)
        {
            _commandSender = commandSender;
            _repository = repository;
            _readModel = readModel;
            _authenticationTaskFactory = authenticationFactory;

            _currentVersion = -1;

            _addedSensorDto = new ConcurrentBag<SensorDto>();
            _addedEventDescriptors = new ConcurrentBag<EventDescriptors>();
            _addedSample = new ConcurrentBag<SampleDto>();
            _eventDatabase = new TrackingDatabaseDecorator<EventDescriptors>(
                eventDatabase, 
                e => _addedEventDescriptors.Add(e));
            _sensorsDatabase = new TrackingDatabaseDecorator<SensorDto>(
                sensorReadDatabase,
                e => _addedSensorDto.Add(e));
            _samplesDatabase = new TrackingDatabaseDecorator<SampleDto>(
                samplesReadDatabase,
                e => _addedSample.Add(e));
        }

        public UserFixture CreatePerson(Guid id)
        {
            PersonId = id;
            _commandSender.Send(
                new CreatePersonCommand(
                    PersonId, 
                    _currentVersion++));

            return this;
        }

        public async Task CreateToken<TService>()
            where TService : ResourceProvider, new()
        {
            //var task = _authenticationTaskFactory.GenerateTask<TService>(
            //    PersonId, 
            //    _currentVersion++,
            //    context);
            //await task.Execute();

            var token = TestSettings.GetToken<TService>();
            //Fake this out so that we don't have to worry about the UI or interaction
            await _commandSender.Send(
                new UpdateTokenCommand<TService>(
                    PersonId,
                    token as JObject,
                    _currentVersion++))
                .ConfigureAwait(false);
        }

        public void CreateSensor<TRequest, TService>(
            TimeSpan pollingInterval)
            where TRequest : Request, new()
            where TService : ResourceProvider, new()
        {
            _commandSender.Send(
                new CreateSensorCommand<TRequest>(
                    PersonId, 
                    _currentVersion++,
                    pollingInterval));
        }

        public void DeactivateASensor<TRequest, TService>()
            where TRequest : Request, new()
            where TService : ResourceProvider, new()
        {
            var command = new DeactivateSensorCommand<TRequest>(PersonId, _currentVersion++);
            _commandSender.Send(command);
        }

        public void ActivateASensor<TRequest, TService>()
            where TRequest : Request, new()
            where TService : ResourceProvider, new()
        {
            var command = new ReactivateSensorCommand<TRequest>(PersonId, _currentVersion++);
            _commandSender.Send(command);
        }

        public void AssertPersonIdIsAppSettingPersonId()
        {
            Assert.Equal(
                PersonId.ToString(), 
                UserSettings.UserId.ToString());
        }

        public WriteRepresentation WriteRepositoryRepresentation<TRequest, TService>()
            where TRequest : Request, new()
            where TService : ResourceProvider, new()
        {
            var person = _repository.GetById(PersonId);
            var tokens = person.GetMemberValue<ICollection<Token>>("_tokens");
            var token = tokens.SingleOrDefault(t => t.ServiceName == typeof (TService).Name);
            
            var sensors = person.GetMemberValue<ICollection<Sensor>>("_sensors");
            var sensor = sensors.SingleOrDefault(s => s.GetType().Name == typeof (Sensor<TRequest>).Name);

            return new WriteRepresentation(token, sensor);
        }

        public SensorDto SensorReadRepositoryRepresentation<TRequest>()
            where TRequest : Request
        {
            return _readModel
                .GetSensors(PersonId)
                .SingleOrDefault(s => s.SensorType.Name == typeof(TRequest).Name);
        }

        public IEnumerable<SampleDto<TRequest>> SampleReadRepositoryRepresentation<TRequest>()
            where TRequest : Request
        {
            return _readModel.GetSamples<TRequest>(PersonId);
        }

        public void RetainUserOnDispose()
        {
            _disposeOfUser = false;
        }

        public void Dispose()
        {
            if (_disposeOfUser)
            {
                foreach (var entry in _addedEventDescriptors)
                {
                    _eventDatabase.Delete(entry.Id.ToString());
                }
                foreach (var entry in _addedSample)
                {
                    _samplesDatabase.Delete(entry.Id.ToString());
                }
                foreach (var entry in _addedSensorDto)
                {
                    _sensorsDatabase.Delete(entry.Id.ToString());
                }
            }

#if !__MOBILE__
            System.Configuration.ConfigurationManager.AppSettings["myId"] = "";
#endif
        }
    }

    public class WriteRepresentation
    {
        public Token Token { get; }
        public Sensor Sensor { get; }

        public WriteRepresentation(
            Token token, 
            Sensor sensor)
        {
            Token = token;
            Sensor = sensor;
        }
    }
}
