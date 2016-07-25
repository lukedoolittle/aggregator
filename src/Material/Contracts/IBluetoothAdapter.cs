using System;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Material.Contracts
{
    public interface IBluetoothAdapter
    {
        void ListDevices(Action<BluetoothDevice> newDeviceFound);

        Task<bool> ConnectToDevice();

        Task<bool> ConnectToDevice(Guid address);

        Task<Tuple<DateTimeOffset, JObject>> GetCharacteristicValue(
            Guid deviceAddress,
            Guid serviceUuid,
            Guid characteristicUuid);
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
