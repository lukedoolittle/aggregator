#if __MOBILE__
using System;
using System.Collections.Generic;
using System.Linq;
using Material;
using Newtonsoft.Json.Linq;
using Material.Contracts;
using Material.Exceptions;
using Robotics.Mobile.Core.Bluetooth.LE;
using Material.Framework;

namespace Aggregator.Infrastructure.Adapters
{
    using System.Threading.Tasks;

    public class BluetoothAdapter : IBluetoothAdapter
    {
        private readonly IAdapter _adapter;
        private readonly List<Guid> _connectingAddresses; 
         
        public BluetoothAdapter(IAdapter adapter)
        {
            _adapter = adapter;

            _connectingAddresses = new List<Guid>();
        }

        public async Task<bool> ConnectToDevice()
        {
            return await ConnectToDevice(Guid.Empty)
                .ConfigureAwait(true);
        }

        public async Task<bool> ConnectToDevice(Guid deviceAddress)
        {
            var device = await Connect(deviceAddress)
                .ConfigureAwait(true);
            return device != null;
        }

        public void ListDevices(Action<BluetoothDevice> newDeviceFound)
        {
            _adapter.DeviceDiscovered += (sender, args) => {
                Platform.RunOnMainThread(() => {
                    newDeviceFound(
                        new BluetoothDevice(
                            args.Device.
                            Name,args.Device.ID));
                });
            };

            _adapter.ScanTimeoutElapsed += (sender, args) =>
            {
                Platform.RunOnMainThread(() => {
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
                    Platform.RunOnMainThread(() => {
                        _adapter.ConnectToDevice(args.Device);
                    });
                }
            };

            _adapter.ScanTimeoutElapsed += (sender, args) =>
            {
                Platform.RunOnMainThread(() => {
                    _adapter.StopScanningForDevices();
                    if (_connectingAddresses.Count == 0)
                    {
                        taskCompletionSource.SetResult(null);                    
                    }
                });
            };

            _adapter.DeviceConnected += (sender, args) =>
            {
                Platform.RunOnMainThread(() =>
                {
                    _connectingAddresses.Remove(deviceAddress);
                    taskCompletionSource.SetResult(args.Device);
                });
            };

            _adapter.StartScanningForDevices(deviceAddress);

            return taskCompletionSource.Task;
        }

        public Task<Tuple<DateTimeOffset, JObject>> GetCharacteristicValue(
            Guid deviceAddress,
            Guid serviceUuid,
            Guid characteristicUuid)
        {
            var taskCompletionSource = new TaskCompletionSource<Tuple<DateTimeOffset, JObject>>();

            Platform.RunOnMainThread(async () =>
            {
                var device = await Connect(deviceAddress)
                .ConfigureAwait(true);

                if (device == null)
                {
                    throw new ConnectivityException(string.Format(
                        StringResources.BluetoothConnectivityException,
                        deviceAddress.ToString()));
                }

                var desiredCharacteristic = device
                    .Services
                    ?.FirstOrDefault(s => s.ID == serviceUuid)
                    ?.Characteristics
                    ?.FirstOrDefault(c => c.ID == characteristicUuid);

                string value = null;

                if (desiredCharacteristic != null)
                {
                    value = await GetCharacteristicValue(
                        desiredCharacteristic)
                        .ConfigureAwait(true);
                }
                else
                {
                    value = await GetCharacteristicValue(
                        device,
                        serviceUuid,
                        characteristicUuid)
                        .ConfigureAwait(true);
                }

                var timestamp = DateTimeOffset.Now;

                taskCompletionSource.SetResult(
                    new Tuple<DateTimeOffset, JObject>(
                        timestamp,
                        new JObject
                        {
                            ["heartrate"] = value,
                            ["timestamp"] = timestamp
                        }));
            });

            return taskCompletionSource.Task;
        }

        private Task<string> GetCharacteristicValue(
            IDevice device,
            Guid serviceUuid,
            Guid characteristicUuid)
        {
            var taskCompletionSource = new TaskCompletionSource<string>();

            device.ServicesDiscovered +=
                (servicesSender, servicesEventArgs) =>
                {
                    Platform.RunOnMainThread(() =>
                    {
                        foreach (var service in device.Services)
                        {
                            if (service.ID == serviceUuid)
                            {
                                service.CharacteristicsDiscovered +=
                                    (characteristicsSender, characteristicsEventArgs) =>
                                    {
                                        Platform.RunOnMainThread(async () =>
                                        {
                                            foreach (var characteristic in service.Characteristics)
                                            {
                                                if (characteristic.ID == characteristicUuid)
                                                {
                                                    taskCompletionSource.SetResult(
                                                        await GetCharacteristicValue(
                                                            characteristic).ConfigureAwait(true));
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

        private Task<string> GetCharacteristicValue(ICharacteristic characteristic)
        {
            var taskCompletionSource = new TaskCompletionSource<string>();

            EventHandler<CharacteristicReadEventArgs> handler = null;
            handler = (sender, args) =>
                {
                    Platform.RunOnMainThread(() =>
                    {
                        characteristic.ValueUpdated -= handler;
                        characteristic.StopUpdates();
                        if (!taskCompletionSource.Task.IsCompleted)
                        {
                            taskCompletionSource.SetResult(
                                DecodeCharacteristic(
                                    args.Characteristic.Value));
                        }
                    });
                };

            characteristic.ValueUpdated += handler;
            characteristic.StartUpdates();

            return taskCompletionSource.Task;
        }

        //TODO: this should be broken out and injected since it only works
        //for heart rate specific values
        private static string DecodeCharacteristic(byte[] data)
        {
            ushort bpm = 0;
            if ((data[0] & 0x01) == 0)
            {
                bpm = data[1];
            }
            else
            {
                bpm = (ushort)data[1];
                bpm = (ushort)(((bpm >> 8) & 0xFF) | ((bpm << 8) & 0xFF00));
            }
            return bpm.ToString();
        }
    }
}
#endif