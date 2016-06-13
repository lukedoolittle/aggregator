using System;
using Aggregator.Domain.Write;
using SimpleCQRS.Domain;

namespace Aggregator.Domain.Events
{
    public class SensorDeactivated<TRequest> : Event
        where TRequest : Request, new()
    {
        public SensorDeactivated(Guid sensorId)
        {
            SensorId = sensorId;
        }

        public Guid SensorId { get; private set; }
    }
}
