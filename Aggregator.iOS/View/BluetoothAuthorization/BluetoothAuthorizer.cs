using System;
using System.Threading.Tasks;
using Aggregator.Framework.Contracts;
using UIKit;

namespace Aggregator.View.BluetoothAuthorization
{
    public class BluetoothAuthorizer : IBluetoothAuthorizer
    {
        private readonly IBluetoothManager _manager;
        private readonly UIViewController _context;

        public BluetoothAuthorizer(
            IBluetoothManager manager,
            UIViewController context)
        {
            _manager = manager;
            _context = context;
        }

        public Task<Guid> GetDeviceUuid()
        {
            var taskCompletionSource = new TaskCompletionSource<Guid>();
            var controller = new DeviceTableViewController(_context.View.Bounds);
            _context.PresentViewController(controller, false, null);

            _manager.ListDevices(controller.OnDeviceFound);
            controller.DeviceSelected = device =>
            {
                taskCompletionSource.SetResult(device.Address);
                controller.DismissViewController(false, null);
            };

            return taskCompletionSource.Task;
        }
    }
}
