using System;
using System.Runtime.Serialization;

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
        public Guid DeviceAddress { get; }

        public override string ExpiresIn => "0";

        public override bool AreValidIntermediateCredentials => true;
    }
}
