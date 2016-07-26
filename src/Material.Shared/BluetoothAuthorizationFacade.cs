#if __MOBILE__
using System.Threading.Tasks;
using Material.Infrastructure;
using Material.Infrastructure.Credentials;
using Material.Adapters;
using Material.Framework;
using Material.View.BluetoothAuthorization;

namespace Material
{
    public class BluetoothAuthorizationFacade<TResourceProvider>
        where TResourceProvider : BluetoothResourceProvider, new()
    {
        public Task<BluetoothCredentials> GetBluetoothCredentials()
        {
            var authenticationUI = new BluetoothAuthorizerUI(
                new BluetoothAdapter(Platform.BluetoothAdapter));

            return authenticationUI.GetDeviceUuid();
        }
    }
}
#endif
