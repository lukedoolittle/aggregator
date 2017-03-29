#if __MOBILE__
using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;
using Material.Contracts;
using Material.Framework;
using Robotics.Mobile.Core.Bluetooth.LE;
using Material.Bluetooth.SubscriptionManagers;
using Material.Domain.Bluetooth;
using Material.Framework.Exceptions;

namespace Material.Bluetooth
{
    /// <summary>
    /// Interaction with Bluetooth devices
    /// Note that this class is NOT threadsafe, but neither is IAdapter, at least on iOS
    /// </summary>
    public class BluetoothAdapter : IBluetoothAdapter
    {
        private readonly IAdapter _adapter;
        private readonly ConcurrentBag<Guid> _connectingAddresses = new ConcurrentBag<Guid>();
        private readonly AdapterSubscriptionManager _subscriptionManager;

        public BluetoothAdapter(IAdapter adapter)
        {
            _adapter = adapter;
            _subscriptionManager = new AdapterSubscriptionManager(adapter);
        }

        #region Listing

        public void ListDevices(Action<BluetoothDevice> newDeviceFound)
        {
            foreach (var device in _adapter.ConnectedDevices)
            {
                newDeviceFound(
                    new BluetoothDevice(
                        device.Name,
                        device.ID));
            }

            _subscriptionManager.AddDiscoveredHandler((sender, args) => {
                Platform.Current.RunOnMainThread(() => {
                    newDeviceFound(
                        new BluetoothDevice(
                            args.Device.Name,
                            args.Device.ID));
                });
            });

            _subscriptionManager.AddTimeoutHandler((sender, args) =>
            {
                _subscriptionManager.Unsubscribe();
                Platform.Current.RunOnMainThread(() => {
                    _adapter.StopScanningForDevices();
                });
            });
            
            _subscriptionManager.Subscribe();

            _adapter.StartScanningForDevices(Guid.Empty);
        }

        #endregion Listing

        #region Connection

        public Task<bool> ConnectToDevice()
        {
            return ConnectToDevice(default(Guid));
        }

        public async Task<bool> ConnectToDevice(Guid address)
        {
            var device = await Connect(address)
                .ConfigureAwait(false);
            return device != null;
        }

        private Task<IDevice> Connect(Guid deviceAddress)
        {
            _subscriptionManager.Unsubscribe();
            _adapter.StopScanningForDevices();

            var device = _adapter
                .ConnectedDevices
                .FirstOrDefault(d => d.ID == deviceAddress);

            if (device != null)
            {
                return Task.FromResult(device);
            }

            _subscriptionManager.AddDiscoveredHandler((sender, args) => 
            {
                if (args.Device.ID == deviceAddress && 
                    !_connectingAddresses.Contains(deviceAddress))
                {
                    _connectingAddresses.Add(deviceAddress);

                    Platform.Current.RunOnMainThread(() => {
                        _adapter.StopScanningForDevices();
                        _adapter.ConnectToDevice(args.Device);
                    });
                }
            });

            var completionSource = new TaskCompletionSource<IDevice>();

            _subscriptionManager.AddConnectedHandler((sender, args) =>
            {
                Platform.Current.RunOnMainThread(() =>
                {
                    _connectingAddresses.TryTake(out deviceAddress);

                    if (!completionSource.Task.IsCompleted)
                    {
                        _subscriptionManager.Unsubscribe();
                        _adapter.StopScanningForDevices();
                        completionSource.SetResult(args.Device);
                    }
                });
            });

            _subscriptionManager.AddTimeoutHandler((sender, args) =>
            {
                Platform.Current.RunOnMainThread(() => {
                    if (!completionSource.Task.IsCompleted &&
                        _connectingAddresses.Count == 0)
                    {
                        _subscriptionManager.Unsubscribe();
                        _adapter.StopScanningForDevices();

                        completionSource.SetResult(null);                    
                    }
                });
            });

            _subscriptionManager.Subscribe();

            _adapter.StartScanningForDevices(Guid.Empty);

            return completionSource.Task;
        }

        #endregion Connection

        public async Task<ISubscriptionManager> SubscribeToCharacteristicValue(
            GattDefinition gatt,
            Action<byte[]> callback)
        {
            var completionSource = new TaskCompletionSource<ISubscriptionManager>();

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
                    var subscriptionManager = new DeviceSubscriptionManager();
                    subscriptionManager.AddCharacteristicsValueHandler(
                        (sender, args) =>
                        {
                            Platform.Current.RunOnMainThread(() =>
                            {
                                callback(args.Characteristic.Value);
                            });
                        });
                    subscriptionManager.SubscribeToCharacteristicsRead(
                        desiredCharacteristic);

                    completionSource.SetResult(subscriptionManager);
                }
                else
                {
                    var result = await SubscribeToCharacteristicValue(
                            device,
                            gatt.ServiceUuid,
                            gatt.CharacteristicUuid,
                            callback)
                        .ConfigureAwait(false);
                    completionSource.SetResult(result);
                }
            });

            return await completionSource
                .Task
                .ConfigureAwait(false);
        }

        private static Task<ISubscriptionManager> SubscribeToCharacteristicValue(
            IDevice device,
            Guid serviceUuid,
            Guid characteristicUuid,
            Action<byte[]> callback)
        {
            var taskCompletionSource = new TaskCompletionSource<ISubscriptionManager>();

            var subscriptionManager = new DeviceSubscriptionManager();

            subscriptionManager.AddServicesDiscoveredHandler(
                (servicesSender, servicesEventArgs) =>
                {
                    Platform.Current.RunOnMainThread(() =>
                    {
                        foreach (var service in device.Services)
                        {
                            if (service.ID == serviceUuid)
                            {
                                subscriptionManager.AddCharacteristicsDiscoveredHandler(
                                    (characteristicsSender, characteristicsEventArgs) =>
                                    {
                                        Platform.Current.RunOnMainThread(() =>
                                        {
                                            foreach (var characteristic in service.Characteristics)
                                            {
                                                if (characteristic.ID == characteristicUuid)
                                                {
                                                    subscriptionManager.AddCharacteristicsValueHandler(
                                                        (sender, args) =>
                                                        {
                                                            Platform.Current.RunOnMainThread(() =>
                                                            {
                                                                callback(args.Characteristic.Value);
                                                            });
                                                        });
                                                    subscriptionManager.SubscribeToCharacteristicsRead(
                                                        characteristic);
                                                    taskCompletionSource.SetResult(subscriptionManager);
                                                }
                                            }
                                        });
                                    });
                                subscriptionManager
                                    .SubscribeToCharacteristicDiscovered(service);
                            }
                        }
                    });
                });

            subscriptionManager
                .SubscribeToServiceDiscovered(device);

            return taskCompletionSource.Task;
        }

    }
}
#endif