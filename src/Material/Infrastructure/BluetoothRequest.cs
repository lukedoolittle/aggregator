using System;
using Material.Infrastructure.Bluetooth;

namespace Material.Infrastructure
{
    //TODO: should override GetHashCode() for this value object
    public abstract class BluetoothRequest : Request
    {
        public abstract BluetoothSpecification Service { get; }
        public abstract BluetoothSpecification Characteristic { get; }
        public abstract Func<byte[], string> CharacteristicConverter { get; }
    }
}
