#if __MOBILE__
using System;
using System.Threading.Tasks;
using Material.Adapters;
using Material.Infrastructure;
using Material.Infrastructure.Credentials;
using Material.Infrastructure.Requests;
using Material.Framework;

namespace Material
{
    public class BluetoothRequester
    {
        /// <summary>
        /// Make a request for a single piece of data from a Bluetooth provider
        /// </summary>
        /// <typeparam name="TRequest">Request type for provider</typeparam>
        /// <param name="credentials">Credentials for provider</param>
        /// <returns>Resource from provider</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        public async Task<BluetoothResponse> MakeBluetoothRequestAsync<TRequest>(
            BluetoothCredentials credentials)
            where TRequest : BluetoothRequest, new()
        {
            var request = new TRequest();

            var result = await new BluetoothAdapter(Platform.Current.BluetoothAdapter)
                .GetCharacteristicValue(
                    credentials.DeviceAddress,
                    request.Service.AssignedNumber,
                    request.Characteristic.AssignedNumber)
                .ConfigureAwait(false);

            var value = new TRequest().CharacteristicConverter(result);
            return new BluetoothResponse
            {
                Reading = value,
                Timestamp = DateTimeOffset.Now
            };
        }
    }
}
#endif
