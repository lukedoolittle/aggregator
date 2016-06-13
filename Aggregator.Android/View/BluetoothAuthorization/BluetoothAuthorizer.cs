using System;
using System.Threading.Tasks;
using Android.Content;
using Aggregator.Framework.Contracts;
using Aggregator.Framework.Exceptions;

namespace Aggregator.View.BluetoothAuthorization
{
    public class BluetoothAuthorizer : IBluetoothAuthorizer
    {
        private readonly IBluetoothManager _manager;
        private readonly Context _context;

        public BluetoothAuthorizer(
            IBluetoothManager manager, 
            Context context)
        {
            _manager = manager;
            _context = context;
        }

        public async Task<Guid> GetDeviceUuid()
        {
            var activityCompletionSource = new TaskCompletionSource<DeviceListActivity>();
            var addressCompletionSource = new TaskCompletionSource<Guid>();

            var intent = new Intent(_context, typeof(DeviceListActivity));
            //TODO: remove magic strings
            intent.PutExtra(
                "Authorizer",
                DeviceListActivity.StateRepo.Add(activityCompletionSource));
            _context.StartActivity(intent);

            var activity = await activityCompletionSource.Task.ConfigureAwait(false);

            _manager.ListDevices(activity.OnDeviceFound);

            activity.DeviceSelected = async device =>
            {
                var result = await _manager
                    .ConnectToDevice(device.Address)
                    .ConfigureAwait(false);

                if (result)
                {
                    addressCompletionSource.SetResult(device.Address);
                }
                else
                {
                    throw new ConnectivityException();
                }
            };

            return await addressCompletionSource.Task.ConfigureAwait(false);
        }
    }
}