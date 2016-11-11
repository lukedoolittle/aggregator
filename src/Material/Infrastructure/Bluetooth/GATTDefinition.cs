using System;
using System.Globalization;

namespace Material.Infrastructure.Bluetooth
{
    public class GattDefinition
    {
        public GattDefinition(
            Guid deviceAddress,
            int serviceUuid,
            int characteristicUuid)
        {
            DeviceAddress = deviceAddress;
            ServiceUuid = UuidFromPartial(serviceUuid);
            CharacteristicUuid = UuidFromPartial(characteristicUuid);
        }

        public GattDefinition(
            Guid deviceAddress, 
            Guid serviceUuid, 
            Guid characteristicUuid)
        {
            DeviceAddress = deviceAddress;
            ServiceUuid = serviceUuid;
            CharacteristicUuid = characteristicUuid;
        }

        public Guid DeviceAddress { get; }
        public Guid ServiceUuid { get; }
        public Guid CharacteristicUuid { get; }

        private static Guid UuidFromPartial(int partial)
        {
            var input = partial.ToString("X", CultureInfo.InvariantCulture).PadRight(4, '0');
            if (input.Length == 4)
                input = "0000" + input + "-0000-1000-8000-00805f9b34fb";
            var guid = Guid.ParseExact(input, "d");
            return guid;
        }
    }
}
