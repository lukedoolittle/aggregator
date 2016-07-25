using System;
using Newtonsoft.Json.Linq;
using SimpleCQRS.Domain;

namespace Aggregator.Domain.Read
{
    public class SensorDto : Entity
    {
        public Guid AggregateId { get; set; }
        public JObject Token { get; set; }
        public Type ServiceType { get; private set; }
        public TimeSpan PollingInterval { get; set; }
        public Type SensorType { get; private set; }
        public bool IsActive { get; set; }
        public string LastSample { get; set; }
        public int Version { get; set; }

        public SensorDto(
            Guid id,
            Guid aggregateId,
            TimeSpan pollingInterval,
            JObject token,
            Type serviceType,
            Type sensorType,
            string lastSample,
            int version)
        {
            Id = id;
            AggregateId = aggregateId;
            Token = token;
            ServiceType = serviceType;
            PollingInterval = pollingInterval;
            SensorType = sensorType;
            LastSample = lastSample;
            Version = version;

            IsActive = true;
        }
    }
}
