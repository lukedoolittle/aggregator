using System;

namespace Material.Domain.Responses
{
    public class BluetoothResponse
    {
        public string Reading { get; set; }
        public DateTimeOffset Timestamp { get; set; }
    }
}
