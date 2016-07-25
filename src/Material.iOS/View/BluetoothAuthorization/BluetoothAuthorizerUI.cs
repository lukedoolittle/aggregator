using System;
using System.Threading.Tasks;
using Aggregator.Framework;
using Material.Contracts;
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
            var taskCompletionSource = new TaskCompletionSource<Guid>();
            var context = Platform.Context;

            var controller = new DeviceTableViewController(context.View.Bounds);
            context.PresentViewController(controller, false, null);

            _adapter.ListDevices(controller.OnDeviceFound);
            controller.DeviceSelected = device =>
            {
                taskCompletionSource.SetResult(device.Address);
                controller.DismissViewController(false, null);
            };

            var uuid = await taskCompletionSource.Task.ConfigureAwait(false);

            return new BluetoothCredentials(uuid);
        }
    }
}
