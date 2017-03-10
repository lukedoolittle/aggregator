using System.Threading.Tasks;
using Material.Contracts;
using Material.Infrastructure.Credentials;

namespace Material.Infrastructure
{
    public class BluetoothApp<TResourceProvider>
        where TResourceProvider : BluetoothResourceProvider, new()
    {
        private readonly IBluetoothAuthorizerUIFactory _factory;

        public BluetoothApp(IBluetoothAuthorizerUIFactory factory)
        {
            _factory = factory;
        }

        public BluetoothApp() : 
            this (QuantfabricConfiguration.BluetoothAuthorizationUIFactory)
        { }

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
