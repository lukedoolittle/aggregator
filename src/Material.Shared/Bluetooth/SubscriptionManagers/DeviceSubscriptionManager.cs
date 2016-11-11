#if __MOBILE__
using System;
using Material.Contracts;
using Robotics.Mobile.Core.Bluetooth.LE;

namespace Material.Bluetooth.SubscriptionManagers
{
    public class DeviceSubscriptionManager : ISubscriptionManager
    {
        private IDevice _device;
        private IService _service;
        private ICharacteristic _characteristic;

        private EventHandler _servicesDiscovered;
        private EventHandler _characteristicsDiscovered;
        private EventHandler<CharacteristicReadEventArgs> _characteristicsRead;

        public DeviceSubscriptionManager AddServicesDiscoveredHandler(
            EventHandler servicesDiscovered)
        {
            if (servicesDiscovered == null) throw new ArgumentNullException(nameof(servicesDiscovered));

            _servicesDiscovered = servicesDiscovered;

            return this;
        }

        public DeviceSubscriptionManager AddCharacteristicsDiscoveredHandler(
            EventHandler characteristicsDiscovered)
        {
            if (characteristicsDiscovered == null) throw new ArgumentNullException(nameof(characteristicsDiscovered));

            _characteristicsDiscovered = characteristicsDiscovered;

            return this;
        }

        public DeviceSubscriptionManager AddCharacteristicsValueHandler(
            EventHandler<CharacteristicReadEventArgs> characteristicsRead)
        {
            if (characteristicsRead == null) throw new ArgumentNullException(nameof(characteristicsRead));

            _characteristicsRead = characteristicsRead;

            return this;
        }

        public void SubscribeToServiceDiscovered(IDevice device)
        {
            if (device == null) throw new ArgumentNullException(nameof(device));

            _device = device;
            _device.ServicesDiscovered += _servicesDiscovered;
            _device.DiscoverServices();
        }

        public void SubscribeToCharacteristicDiscovered(IService service)
        {
            if (service == null) throw new ArgumentNullException(nameof(service));

            if (_device != null)
            {
                _device.ServicesDiscovered -= _servicesDiscovered;
                _servicesDiscovered = null;
            }

            _service = service;
            _service.CharacteristicsDiscovered += _characteristicsDiscovered;
            _service.DiscoverCharacteristics();
        }

        public void SubscribeToCharacteristicsRead(ICharacteristic characteristic)
        {
            if (characteristic == null) throw new ArgumentNullException(nameof(characteristic));

            if (_service != null)
            {
                _service.CharacteristicsDiscovered -= _characteristicsDiscovered;
                _characteristicsDiscovered = null;
            }
            
            _characteristic = characteristic;
            _characteristic.ValueUpdated += _characteristicsRead;
            _characteristic.StartUpdates();
        }

        void ISubscriptionManager.Unsubscribe()
        {
            if (_characteristic != null)
            {
                _characteristic.ValueUpdated -= _characteristicsRead;
                _characteristic.StopUpdates();
                _characteristicsRead = null;
            }
        }
    }
}
#endif
