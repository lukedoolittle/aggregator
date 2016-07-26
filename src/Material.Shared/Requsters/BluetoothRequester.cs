#if __MOBILE__
using System;
using System.Threading.Tasks;
using Material.Adapters;
using Material.Infrastructure;
using Material.Infrastructure.Credentials;
using Material.Infrastructure.Static;
using Material.Framework;

namespace Material
{
    public class BluetoothRequester
    {
        public async Task<BluetoothResponse> MakeBluetoothRequest<TRequest>(
            BluetoothCredentials credentials)
            where TRequest : BluetoothRequest, new()
        {
            var request = new TRequest();

            var result = await new BluetoothAdapter(Platform.BluetoothAdapter)
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
