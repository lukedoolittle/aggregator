using System;
using System.Threading.Tasks;
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
            var taskCompletionSource = new TaskCompletionSource<Guid>();
            var context = Platform.Current.Context;

            var controller = new DeviceTableViewController(
                context.View.Bounds);
            context.PresentViewController(
                controller, 
                false, 
                null);

            _adapter.ListDevices(controller.OnDeviceFound);
            controller.SetDeviceSelectedHandler(async device =>
            {
                controller.DisplayLoading(device.Name);

                var result = await _adapter
                    .ConnectToDevice(device.Address)
                    .ConfigureAwait(false);

                if (result)
                {
                    taskCompletionSource.SetResult(device.Address);
                    Platform.Current.RunOnMainThread(() =>
                    {
                        controller.HideLoading();
                        controller.DismissViewController(
                            false,
                            null);
                    });
                }
                else
                {
                    throw new NoConnectivityException(string.Format(
                        StringResources.BluetoothConnectivityException,
                        device.Address.ToString()));
                }
            });

            var uuid = await taskCompletionSource
                .Task
                .ConfigureAwait(false);

            return new BluetoothCredentials(uuid);
        }
    }
}
