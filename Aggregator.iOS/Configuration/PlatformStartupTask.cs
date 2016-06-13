using BatmansBelt;
using CoreLocation;
using Plugin.Geolocator;
using Aggregator.Framework;
using UIKit;

namespace Aggregator.Configuration
{
    public class PlatformStartupTask : IStartupTask
    {
        public void Execute()
        {
            new CLLocationManager().RequestAlwaysAuthorization();
            CrossGeolocator.Current.AllowsBackgroundUpdates = true;

            Platform.RunOnMainThread =
                UIApplication.SharedApplication.InvokeOnMainThread;
        }
    }
}
