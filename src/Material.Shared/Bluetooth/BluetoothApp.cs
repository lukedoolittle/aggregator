#if __MOBILE__
using Material.Bluetooth;
using Material.Infrastructure;

namespace Material.Bluetooth
{
    public class BluetoothApp<TResourceProvider> : BluetoothAppBase<TResourceProvider>
        where TResourceProvider : BluetoothResourceProvider, new()
    {
        public BluetoothApp() : base(new BluetoothAuthorizerUIFactory())
        { }
    }
}
#endif
