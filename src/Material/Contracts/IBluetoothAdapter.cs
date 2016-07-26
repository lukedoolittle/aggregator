using System;
using System.Threading.Tasks;

namespace Material.Contracts
{
    public interface IBluetoothAdapter
    {
        void ListDevices(Action<BluetoothDevice> newDeviceFound);

        Task<bool> ConnectToDevice(Guid address = default(Guid));

        Task<byte[]> GetCharacteristicValue(
            Guid deviceAddress,
            int serviceUuid,
            int characteristicUuid);
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
