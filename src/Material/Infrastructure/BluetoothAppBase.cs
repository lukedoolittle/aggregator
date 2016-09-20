using System.Threading.Tasks;
using Material.Infrastructure;
using Material.Infrastructure.Credentials;
using Material.Contracts;

namespace Material
{
    public class BluetoothAppBase<TResourceProvider>
        where TResourceProvider : BluetoothResourceProvider, new()
    {
        private readonly IBluetoothAuthorizerUIFactory _factory;

        public BluetoothAppBase(IBluetoothAuthorizerUIFactory factory)
        {
            _factory = factory;
        }

        /// <summary>
        /// Allows user to select bluetooth device
        /// </summary>
        /// <returns>Bluetooth credentials</returns>
        public Task<BluetoothCredentials> GetBluetoothCredentialsAsync()
        {
            var authenticationUI = _factory.GetAuthorizer();

            return authenticationUI.GetDeviceUuid();
        }
    }
}
