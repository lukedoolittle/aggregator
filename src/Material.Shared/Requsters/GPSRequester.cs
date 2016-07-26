#if __MOBILE__
using System.Linq;
using Foundations.Serialization;
using Material.Adapters;
using Plugin.Geolocator;
#if __IOS__
using CoreLocation;
#endif

namespace Material
{
    using System.Threading.Tasks;
    public class GPSRequester
    { 
        public GPSRequester()
        {
#if __IOS__
            new CLLocationManager().RequestAlwaysAuthorization();
            CrossGeolocator.Current.AllowsBackgroundUpdates = true;
#endif
        }

        public static async Task<string> MakeGPSRequest()
        {
            var result = await new GPSAdapter(CrossGeolocator.Current)
                .GetPosition()
                .ConfigureAwait(false);

            return result.First().Item2.AsJson(false);
        }
    }
}
#endif
