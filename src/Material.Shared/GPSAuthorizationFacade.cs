#if __MOBILE__
using System.Threading.Tasks;
using Material.Exceptions;
#if __IOS__
using CoreLocation;
using Plugin.Geolocator;
#endif

namespace Material
{
    public class GPSAuthorizationFacade
    {
        public Task AuthorizeContinuousGPSUsage()
        {
#if __ANDROID__
            return Task.FromResult(new object());
#elif __IOS__
            var taskCompletionSource = new TaskCompletionSource<object>();
            var locationManager = new CLLocationManager();
            locationManager.AuthorizationChanged += (sender, args) =>
            {
                switch (args.Status)
                {
                    case CLAuthorizationStatus.NotDetermined:
                        break;
                    case CLAuthorizationStatus.AuthorizedAlways:
                        taskCompletionSource.SetResult(new object());
                        break;
                    default:
                        throw new AuthorizationException(
                            StringResources.GPSAuthorizationException);
                }
            };
            locationManager.RequestAlwaysAuthorization(); 
            CrossGeolocator.Current.AllowsBackgroundUpdates = true;
            return taskCompletionSource.Task;
#else
            throw new Exception();
#endif
        }
    }
}
#endif
