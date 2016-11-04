#if __MOBILE__
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Material.Contracts;
using Material.Exceptions;
using Material.Framework;
using Robotics.Mobile.Core.Bluetooth.LE;

namespace Material.Adapters
{
    public class BluetoothAdapter : IBluetoothAdapter
    {
        private readonly IAdapter _adapter;
        private readonly List<Guid> _connectingAddresses = 
            new List<Guid>();

        public BluetoothAdapter(IAdapter adapter)
        {
            _adapter = adapter;
        }

        public Task<bool> ConnectToDevice()
        {
            return ConnectToDevice(default(Guid));
        }

        public async Task<bool> ConnectToDevice(Guid address)
        {
            var device = await Connect(address)
                .ConfigureAwait(true);
            return device != null;
        }

        public void ListDevices(Action<BluetoothDevice> newDeviceFound)
        {
            _adapter.DeviceDiscovered += (sender, args) => {
                Platform.Current.RunOnMainThread(() => {
                    newDeviceFound(
                        new BluetoothDevice(
                            args.Device.
                            Name,args.Device.ID));
                });
            };

            _adapter.ScanTimeoutElapsed += (sender, args) =>
            {
                Platform.Current.RunOnMainThread(() => {
                    _adapter.StopScanningForDevices();
                });
            };

            _adapter.StartScanningForDevices(Guid.Empty);
        }

        private Task<IDevice> Connect(Guid deviceAddress)
        {
            var device = _adapter
                .ConnectedDevices
                .FirstOrDefault(d => d.ID == deviceAddress);

            if (device != null)
            {
                return Task.FromResult(device);
            }

            var taskCompletionSource = new TaskCompletionSource<IDevice>();

            _adapter.DeviceDiscovered += (sender, args) => {
                if (args.Device.ID == deviceAddress && 
                    !_connectingAddresses.Contains(deviceAddress))
                {
                    _adapter.StopScanningForDevices();
                    _connectingAddresses.Add(deviceAddress);
                    Platform.Current.RunOnMainThread(() => {
                        _adapter.ConnectToDevice(args.Device);
                    });
                }
            };

            _adapter.ScanTimeoutElapsed += (sender, args) =>
            {
                Platform.Current.RunOnMainThread(() => {
                    _adapter.StopScanningForDevices();
                    if (_connectingAddresses.Count == 0)
                    {
                        taskCompletionSource.SetResult(null);                    
                    }
                });
            };

            _adapter.DeviceConnected += (sender, args) =>
            {
                Platform.Current.RunOnMainThread(() =>
                {
                    _connectingAddresses.Remove(deviceAddress);
                    taskCompletionSource.SetResult(args.Device);
                });
            };

            _adapter.StartScanningForDevices(Guid.Empty);

            return taskCompletionSource.Task;
        }

        public Task<byte[]> GetCharacteristicValue(
            Guid deviceAddress,
            int serviceUuid,
            int characteristicUuid)
        {
            return GetCharacteristicValue(
                deviceAddress, 
                UuidFromPartial(serviceUuid), 
                UuidFromPartial(characteristicUuid));
        }

        public Task<byte[]> GetCharacteristicValue(
            Guid deviceAddress,
            Guid serviceUuid,
            Guid characteristicUuid)
        {
            var taskCompletionSource = new TaskCompletionSource<byte[]>();

            Platform.Current.RunOnMainThread(async () =>
            {
                var device = await Connect(deviceAddress)
                    .ConfigureAwait(true);

                if (device == null)
                {
                    throw new NoConnectivityException(string.Format(
                        StringResources.BluetoothConnectivityException,
                        deviceAddress.ToString()));
                }

                var desiredCharacteristic = device
                    .Services
                    ?.FirstOrDefault(s => s.ID == serviceUuid)
                    ?.Characteristics
                    ?.FirstOrDefault(c => c.ID == characteristicUuid);

                if (desiredCharacteristic != null)
                {
                    var result = await GetCharacteristicValue(
                            desiredCharacteristic)
                        .ConfigureAwait(false);
                    taskCompletionSource.SetResult(result);
                }
                else
                {
                    var result = await GetCharacteristicValue(
                            device,
                            serviceUuid,
                            characteristicUuid)
                        .ConfigureAwait(false);
                    taskCompletionSource.SetResult(result);
                }
            });

            return taskCompletionSource.Task;
        }

        private static Task<byte[]> GetCharacteristicValue(
            IDevice device,
            Guid serviceUuid,
            Guid characteristicUuid)
        {
            var taskCompletionSource = new TaskCompletionSource<byte[]>();

            device.ServicesDiscovered +=
                (servicesSender, servicesEventArgs) =>
                {
                    Platform.Current.RunOnMainThread(() =>
                    {
                        foreach (var service in device.Services)
                        {
                            if (service.ID == serviceUuid)
                            {
                                service.CharacteristicsDiscovered +=
                                    (characteristicsSender, characteristicsEventArgs) =>
                                    {
                                        Platform.Current.RunOnMainThread(async () =>
                                        {
                                            foreach (var characteristic in service.Characteristics)
                                            {
                                                if (characteristic.ID == characteristicUuid)
                                                {
                                                    var result = await GetCharacteristicValue(
                                                            characteristic)
                                                        .ConfigureAwait(false);
                                                    taskCompletionSource.SetResult(result);
                                                }
                                            }
                                        });
                                    };
                                service.DiscoverCharacteristics();
                            }
                        }
                    });
                };

            device.DiscoverServices();

            return taskCompletionSource.Task;
        }

        private static Task<byte[]> GetCharacteristicValue(
            ICharacteristic characteristic)
        {
            var taskCompletionSource = new TaskCompletionSource<byte[]>();

            EventHandler<CharacteristicReadEventArgs> handler = null;
            handler = (sender, args) =>
                {
                    Platform.Current.RunOnMainThread(() =>
                    {
                        characteristic.ValueUpdated -= handler;
                        characteristic.StopUpdates();
                        if (!taskCompletionSource.Task.IsCompleted)
                        {
                            taskCompletionSource.SetResult(
                                    args.Characteristic.Value);
                        }
                    });
                };

            characteristic.ValueUpdated += handler;
            characteristic.StartUpdates();

            return taskCompletionSource.Task;
        }

        private static Guid UuidFromPartial(int partial)
        {
            var input = partial.ToString("X", CultureInfo.InvariantCulture).PadRight(4, '0');
            if (input.Length == 4)
                input = "0000" + input + "-0000-1000-8000-00805f9b34fb";
            var guid = Guid.ParseExact(input, "d");
            return guid;
        }
    }
}
#endif