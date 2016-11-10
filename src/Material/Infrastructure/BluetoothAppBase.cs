using System.Threading.Tasks;
using Material.Contracts;
using Material.Infrastructure.Credentials;

namespace Material.Infrastructure
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
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")]
        public Task<BluetoothCredentials> GetBluetoothCredentialsAsync()
        {
            var authenticationUI = _factory.GetAuthorizer();

            return authenticationUI.GetDeviceUuid();
        }
    }
}
