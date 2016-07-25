using System.Threading.Tasks;
using Material.Infrastructure;
using Material.Infrastructure.Credentials;
#if __MOBILE__
using Aggregator.Infrastructure.Adapters;
using Aggregator.View.BluetoothAuthorization;
using Robotics.Mobile.Core.Bluetooth.LE;
#endif

namespace Material
{

#if __MOBILE__
    public class Bluetooth<TResourceProvider>
        where TResourceProvider : BluetoothResourceProvider, new()
    {
        public Task<BluetoothCredentials> GetBluetoothCredentials()
        {

#if __ANDROID__
            var adapter = new Adapter();
#elif __IOS__
            var adapter = Adapter.Current;
#endif
            var authenticationUI = new BluetoothAuthorizerUI(
                new BluetoothAdapter(adapter));

            return authenticationUI.GetDeviceUuid();

        }
    }
#endif
}
