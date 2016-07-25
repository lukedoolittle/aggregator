using System;
using Newtonsoft.Json;

namespace Material.Infrastructure.Credentials
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
        public override bool HasValidPublicKey => true;

        public override string ExpiresIn => "0";
    }
}
