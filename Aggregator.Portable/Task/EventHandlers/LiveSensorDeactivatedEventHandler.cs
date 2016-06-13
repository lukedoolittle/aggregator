using Aggregator.Domain.Events;
using Aggregator.Domain.Write;
using SimpleCQRS.Framework.Contracts;

namespace Aggregator.Task.EventHandlers
{
    public class LiveSensorDeactivatedEventHandler<TRequest> :
        IEventHandler<SensorDeactivated<TRequest>>
        where TRequest : Request, new()
    {
        private readonly Scheduler _scheduler;

        public LiveSensorDeactivatedEventHandler(Scheduler scheduler)
        {
            _scheduler = scheduler;
        }

        public void Handle(SensorDeactivated<TRequest> @event)
        {
            _scheduler.SuspendTask(@event.SensorId);
        }
    }
}
