using System;
using Newtonsoft.Json;

namespace Aggregator.Infrastructure.Credentials
{
    public class BluetoothCredentials : TokenCredentials
    {
        public BluetoothCredentials() {}

        public BluetoothCredentials(Guid deviceAddress)
        {
            DeviceAddress = deviceAddress;
        }

        [JsonProperty("deviceAddress")]
        public Guid DeviceAddress { get; }

        [JsonIgnore]
        public override bool HasValidProperties => DeviceAddress != Guid.Empty;

        [JsonIgnore]
        public override bool IsTokenExpired => false;
    }
}
