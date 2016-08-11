using System;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Material.Infrastructure.Credentials
{
    [DataContract]
    public class BluetoothCredentials : TokenCredentials
    {
        public BluetoothCredentials() {}

        public BluetoothCredentials(Guid deviceAddress)
        {
            DeviceAddress = deviceAddress;
        }

        [DataMember(Name = "deviceAddress")]
        [JsonProperty("deviceAddress")]
        public Guid DeviceAddress { get; }

        [JsonIgnore]
        public override bool HasValidPublicKey => true;

        public override string ExpiresIn => "0";

        public override bool AreValidIntermediateCredentials => true;
    }
}
