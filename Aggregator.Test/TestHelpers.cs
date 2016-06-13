using Aggregator.Configuration;
using Aggregator.Configuration.Startup;
using Aggregator.Task;
using Aggregator.Task.Factories;
#if __MOBILE__
using Plugin.Geolocator;
using Robotics.Mobile.Core.Bluetooth.LE;
using Aggregator.Infrastructure.ComponentManagers;
#endif

namespace Aggregator.Test
{
    public static class TestHelpers
    {
        public static void SetUpSerialization()
        {
            new SerializationStartupTask().Execute();
        }

        public static ClientFactory CreateClientFactory()
        {
            var oauthFactory = new OAuthFactory();
#if __IOS__
            var gpsManager = new GPSManager(CrossGeolocator.Current);
            var bluetoothManager = new BluetoothManager(Adapter.Current);
            return new ClientFactory(bluetoothManager, gpsManager, oauthFactory);
#elif __ANDROID__
            var gpsManager = new GPSManager(CrossGeolocator.Current);
            var bluetoothManager = new BluetoothManager(new Adapter());
            var smsManager = new AndroidSMSManager();
            return new ClientFactory(smsManager, bluetoothManager, gpsManager, oauthFactory);
#else
            return new ClientFactory(oauthFactory);
#endif
        }

        public static void InitializeCouchbaseDatabase()
        {
            new CouchbaseConfigurationTask().Execute();
        }
    }
}
