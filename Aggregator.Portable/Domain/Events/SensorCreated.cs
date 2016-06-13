using System;
using Newtonsoft.Json.Linq;
using Aggregator.Domain.Write;
using Aggregator.Framework.Enums;
using SimpleCQRS.Domain;

namespace Aggregator.Domain.Events
{
    public class SensorCreated<TRequest> : Event
        where TRequest : Request, new()
    {
        public Guid SensorId { get; private set; }
        public PollingIntervalEnum PollingInterval { get; private set; }
        public JObject AuthenticationToken { get; private set; }
        public bool RequiresAuthentication { get; private set; }

        public SensorCreated(
            Guid sensorId, 
            PollingIntervalEnum pollingInverval,
            JObject authenticationToken,
            bool requiresAuthentication = true)
        {
            SensorId = sensorId;
            PollingInterval = pollingInverval;
            AuthenticationToken = authenticationToken;
            RequiresAuthentication = requiresAuthentication;
        }
    }
}
