using Aggregator.Domain.Events;
using Aggregator.Domain.Write;
using SimpleCQRS.Framework.Contracts;

namespace Aggregator.Task.EventHandlers
{
    public class LiveSensorReactivatedEventHandler<TRequest> :
        IEventHandler<SensorReactivated<TRequest>>
        where TRequest : Request, new()
    {
        private readonly Scheduler _scheduler;

        public LiveSensorReactivatedEventHandler(Scheduler scheduler)
        {
            _scheduler = scheduler;
        }

        public void Handle(SensorReactivated<TRequest> @event)
        {
            _scheduler.ResumeTask(@event.SensorId);
        }
    }
}
