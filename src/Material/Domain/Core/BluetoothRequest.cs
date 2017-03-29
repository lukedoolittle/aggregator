using System;
using Material.Domain.Bluetooth;

namespace Material.Domain.Core
{
    public abstract class BluetoothRequest : Request
    {
        public abstract BluetoothSpecification Service { get; }
        public abstract BluetoothSpecification Characteristic { get; }
        public abstract Func<byte[], string> CharacteristicConverter { get; }
    }
}
