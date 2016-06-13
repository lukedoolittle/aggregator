using System;
using Aggregator.Domain.Write;
using SimpleCQRS.Domain;

namespace Aggregator.Domain.Events
{
    public class SensorReactivated<TRequest> : Event
        where TRequest : Request, new()
    {
        public SensorReactivated(Guid sensorId)
        {
            SensorId = sensorId;
        }

        public Guid SensorId { get; private set; }
    }
}
