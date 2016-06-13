using System;
using Aggregator.Domain.Write;

namespace Aggregator.Infrastructure
{
    public abstract class BluetoothRequest : Request
    {
        public Guid ServiceGuid => UuidFromPartial(ServiceUuid);
        public Guid CharacteristicGuid => UuidFromPartial(CharacteristicUuid);

        public abstract string ServiceUuid { get; }
        public abstract string CharacteristicUuid { get; }

        private static Guid UuidFromPartial(string partialString)
        {
            var partial = Convert.ToInt32(partialString, 16);
            string input = partial.ToString("X").PadRight(4, '0');
            if (input.Length == 4)
                input = "0000" + input + "-0000-1000-8000-00805f9b34fb";
            return Guid.ParseExact(input, "d");
        }
    }
}
