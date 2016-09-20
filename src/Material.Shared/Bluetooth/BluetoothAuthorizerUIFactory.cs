#if __MOBILE__
using Material.Adapters;
using Material.Contracts;
using Material.Framework;
using Material.View.BluetoothAuthorization;

namespace Material.Bluetooth
{
    public class BluetoothAuthorizerUIFactory : IBluetoothAuthorizerUIFactory
    {
        public IBluetoothAuthorizerUI GetAuthorizer()
        {
            return new BluetoothAuthorizerUI(
                new BluetoothAdapter(Platform.Current.BluetoothAdapter));
        }
    }
}
#endif
