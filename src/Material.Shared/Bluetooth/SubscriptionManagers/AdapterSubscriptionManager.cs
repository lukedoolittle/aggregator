#if __MOBILE__
using System;
using Robotics.Mobile.Core.Bluetooth.LE;

namespace Material.Bluetooth.SubscriptionManagers
{
    public class AdapterSubscriptionManager
    {
        private readonly IAdapter _adapter;

        private EventHandler<DeviceDiscoveredEventArgs> _discovered;
        private EventHandler<DeviceConnectionEventArgs> _connected;
        private EventHandler _timeout;

        public AdapterSubscriptionManager(IAdapter adapter)
        {
            if (adapter == null) throw new ArgumentNullException(nameof(adapter));

            _adapter = adapter;
        }

        public AdapterSubscriptionManager AddDiscoveredHandler(
            EventHandler<DeviceDiscoveredEventArgs> discovered)
        {
            if (discovered == null) throw new ArgumentNullException(nameof(discovered));

            _discovered = discovered;

            return this;
        }

        public AdapterSubscriptionManager AddTimeoutHandler(
            EventHandler timeout)
        {
            if (timeout == null) throw new ArgumentNullException(nameof(timeout));

            _timeout = timeout;

            return this;
        }

        public AdapterSubscriptionManager AddConnectedHandler(
            EventHandler<DeviceConnectionEventArgs> connected)
        {
            if (connected == null) throw new ArgumentNullException(nameof(connected));

            _connected = connected;

            return this;
        }

        public void Subscribe()
        {
            _adapter.DeviceDiscovered += _discovered;
            _adapter.ScanTimeoutElapsed += _timeout;
            _adapter.DeviceConnected += _connected;
        }

        public void Unsubscribe()
        {
            _adapter.DeviceDiscovered -= _discovered;
            _adapter.ScanTimeoutElapsed -= _timeout;
            _adapter.DeviceConnected -= _connected;

            _discovered = null;
            _timeout = null;
            _connected = null;
        }
    }
}
#endif
