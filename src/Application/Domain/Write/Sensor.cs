using System;
using Aggregator.Domain.Events;
using Material.Infrastructure;

namespace Aggregator.Domain.Write
{
    public class Sensor<TRequest> : Sensor
        where TRequest : Request, new()
    {
        private TimeSpan _pollingInterval;
#pragma warning disable 414
        private bool _isActive;
#pragma warning restore 414
        
        public Sensor(
            Guid sensorId,
            TimeSpan pollingInterval) :
                base(
                sensorId,
                typeof(TRequest))
        {
            _pollingInterval = pollingInterval;
            _isActive = true;
        }

        #region Event Handlers

        public void OnSensorDeactivated(SensorDeactivated<TRequest> e)
        {
            _isActive = false;
        }

        public void OnSensorReactivated(SensorReactivated<TRequest> e)
        {
            _isActive = true;
        }

        public void OnPollingIntervalChanged(PollingIntervalChanged<TRequest> e)
        {
            _pollingInterval = e.NewPollingInterval;
        }

        #endregion Event Handlers

    }

    public class Sensor
    {
        public Guid SensorId { get; }
        public Type SampleType { get; }

        public Sensor(
            Guid sensorId,
            Type sampleType)
        {
            SensorId = sensorId;
            SampleType = sampleType;
        }
    }
}
