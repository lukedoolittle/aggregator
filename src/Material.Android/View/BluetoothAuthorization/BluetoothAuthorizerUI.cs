using System;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Material.Contracts;
using Material.Exceptions;
using Material.Infrastructure.Credentials;
using Material.Framework;

namespace Material.View.BluetoothAuthorization
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
            intent.PutExtra(
                DeviceListActivity.Authorizer,
                DeviceListActivity.StateRepo.Add(activityCompletionSource));
            context.StartActivity(intent);

            var activity = await activityCompletionSource.Task.ConfigureAwait(false);

            _adapter.ListDevices(activity.OnDeviceFound);

            activity.DeviceSelected = async device =>
            {
                var progressDialog = ProgressDialog.Show(
                    activity,
                    StringResources.BluetoothDialogTitle,
                    string.Format(StringResources.BluetoothDialogBody, device.Name),
                    true);
                var result = await _adapter
                    .ConnectToDevice(device.Address)
                    .ConfigureAwait(false);

                if (result)
                {
                    addressCompletionSource.SetResult(device.Address);
                    activity.Finish();
                    Platform.RunOnMainThread(progressDialog.Hide);
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