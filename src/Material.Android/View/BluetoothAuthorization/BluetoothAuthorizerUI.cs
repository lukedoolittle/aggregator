using System;
using System.Threading.Tasks;
using Material;
using Aggregator.Framework;
using Android.Content;
using Material.Contracts;
using Material.Exceptions;
using Material.Infrastructure.Credentials;

namespace Aggregator.View.BluetoothAuthorization
{
    public class BluetoothAuthorizerUI : IBluetoothAuthorizerUI
    {
        private readonly IBluetoothAdapter _adapter;

        public BluetoothAuthorizerUI(IBluetoothAdapter adapter)
        {
            _adapter = adapter;
        }

        public async Task<BluetoothCredentials> GetDeviceUuid()
        {
            var activityCompletionSource = new TaskCompletionSource<DeviceListActivity>();
            var addressCompletionSource = new TaskCompletionSource<Guid>();
            var context = Platform.Context;

            var intent = new Intent(context, typeof(DeviceListActivity));
            //TODO: remove magic strings
            intent.PutExtra(
                "Authorizer",
                DeviceListActivity.StateRepo.Add(activityCompletionSource));
            context.StartActivity(intent);

            var activity = await activityCompletionSource.Task.ConfigureAwait(false);

            _adapter.ListDevices(activity.OnDeviceFound);

            activity.DeviceSelected = async device =>
            {
                var result = await _adapter
                    .ConnectToDevice(device.Address)
                    .ConfigureAwait(false);

                if (result)
                {
                    addressCompletionSource.SetResult(device.Address);
                    activity.Finish();
                }
                else
                {
                    throw new ConnectivityException(string.Format(
                        StringResources.BluetoothConnectivityException, 
                        device.Address.ToString()));
                }
            };

            var uuid = await addressCompletionSource.Task.ConfigureAwait(false);

            return new BluetoothCredentials(uuid);
        }
    }
}