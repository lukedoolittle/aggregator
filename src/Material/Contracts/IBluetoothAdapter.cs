using System;
using System.Threading.Tasks;
using Material.Domain.Bluetooth;

namespace Material.Contracts
{
    public interface IBluetoothAdapter
    {
        void ListDevices(Action<BluetoothDevice> newDeviceFound);

        Task<bool> ConnectToDevice(Guid address);

        Task<bool> ConnectToDevice();

        Task<ISubscriptionManager> SubscribeToCharacteristicValue(
            GattDefinition gatt,
            Action<byte[]> callback);
    }

    public class BluetoothDevice
    {
        public BluetoothDevice(string name, Guid address)
        {
            Name = name;
            Address = address;
        }

        public string Name { get; }
        public Guid Address { get; }
    }
}
