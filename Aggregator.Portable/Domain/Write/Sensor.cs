using System;
using Aggregator.Domain.Events;
using Aggregator.Framework.Enums;

namespace Aggregator.Domain.Write
{
    public class Sensor<TRequest> : Sensor
        where TRequest : Request, new()
    {
        private PollingIntervalEnum _pollingInterval;
        private bool _isActive;
        
        public Sensor(
            Guid sensorId,
            PollingIntervalEnum pollingInterval) :
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
