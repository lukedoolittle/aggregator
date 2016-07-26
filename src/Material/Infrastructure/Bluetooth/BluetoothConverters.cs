namespace Material.Infrastructure.Bluetooth
{
    public static class BluetoothCharacteristicConverters
    {
        public static string DecodeHeartRateCharacteristic(byte[] data)
        {
            ushort bpm = 0;
            if ((data[0] & 0x01) == 0)
            {
                bpm = data[1];
            }
            else
            {
                bpm = (ushort)data[1];
                bpm = (ushort)(((bpm >> 8) & 0xFF) | ((bpm << 8) & 0xFF00));
            }
            return bpm.ToString();
        }
    }
}
