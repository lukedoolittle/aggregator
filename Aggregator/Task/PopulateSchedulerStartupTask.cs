using System;
using BatmansBelt;
using BatmansBelt.Metadata;
using BatmansBelt.Reflection;
using Aggregator.Configuration.Startup;
using Aggregator.Domain.Read;
using Aggregator.Domain.Write;
using Aggregator.Framework.Contracts;
using Aggregator.Framework.Extensions;
using Aggregator.Task;
using Aggregator.Task.Commands;
using SimpleCQRS.Framework.Contracts;

namespace Aggregator.Configuration
{
    [Dependency(typeof(CouchbaseConfigurationTask))]
    [Dependency(typeof(PlatformStartupTask))]
    public class PopulateSchedulerStartupTask : IStartupTask
    {
        private readonly Scheduler _taskScheduler;
        private readonly ReadModelFacade _readModel; 
        private readonly ISampleRequestTaskFactory _factory;
        private readonly ICommandSender _sender;

        public PopulateSchedulerStartupTask(
            Scheduler taskScheduler,
            ReadModelFacade readModel, 
            ISampleRequestTaskFactory factory,
            ICommandSender sender)
        {
            _taskScheduler = taskScheduler;
            _readModel = readModel;
            _factory = factory;
            _sender = sender;
        }

        public void Execute()
        {
            var myId = UserSettings.UserId;

            if (myId == Guid.Empty)
            {
                UserSettings.UserId = Guid.NewGuid();
                _sender.Send(
                    new CreatePersonCommand(UserSettings.UserId, -1)).Wait();
            }
            else
            {
                var sensors = _readModel.GetSensors(myId);

                foreach (var sensor in sensors)
                {
                    var task = _factory.GetTask(
                        myId,
                        sensor.Token,
                        sensor.SensorType,
                        sensor.LastSample);

                    var request = new InstanceCreator(sensor.SensorType)
                        .Create<Request>();

                    _taskScheduler.AddTask(
                        sensor.Id,
                        task,
                        request.PollingInterval[sensor.PollingInterval],
                        sensor.IsActive);
                }
            }
        }
    }
}
