using System;

namespace Material.Infrastructure.Requests
{
    public class BluetoothResponse
    {
        public string Reading { get; set; }
        public DateTimeOffset Timestamp { get; set; }
    }
}
