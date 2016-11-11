#if __MOBILE__
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Material.Contracts;
using Material.Exceptions;
using Material.Framework;
using Material.Infrastructure.Bluetooth;
using Robotics.Mobile.Core.Bluetooth.LE;

namespace Material.Bluetooth
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

        #region Connection

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

        #endregion Connection

        public Task<byte[]> GetCharacteristicValue(GattDefinition gatt)
        {
            var taskCompletionSource = new TaskCompletionSource<byte[]>();

            Platform.Current.RunOnMainThread(async () =>
            {
                var device = await Connect(gatt.DeviceAddress)
                    .ConfigureAwait(true);

                if (device == null)
                {
                    throw new NoConnectivityException(string.Format(
                        StringResources.BluetoothConnectivityException,
                        gatt.DeviceAddress.ToString()));
                }

                var desiredCharacteristic = device
                    .Services
                    ?.FirstOrDefault(s => s.ID == gatt.ServiceUuid)
                    ?.Characteristics
                    ?.FirstOrDefault(c => c.ID == gatt.CharacteristicUuid);

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
                            gatt.ServiceUuid,
                            gatt.CharacteristicUuid)
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
    }
}
#endif