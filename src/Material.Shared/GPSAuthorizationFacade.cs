#if __MOBILE__
using System.Threading.Tasks;
using Material.Exceptions;
#if __IOS__
using CoreLocation;
using UIKit;
#endif

namespace Material
{
    public class GPSAuthorizationFacade
    {
#if __IOS__
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        public Task<CLLocationManager> AuthorizeContinuousGPSUsage()
#else
        public Task AuthorizeContinuousGPSUsage()
#endif

        {
#if __ANDROID__
            return Task.FromResult(new object());
#elif __IOS__
            var taskCompletionSource = new TaskCompletionSource<CLLocationManager>();
            var locationManager = new CLLocationManager();
            locationManager.AuthorizationChanged += (sender, args) =>
            {
                switch (args.Status)
                {
                    case CLAuthorizationStatus.NotDetermined:
                        break;
                    case CLAuthorizationStatus.AuthorizedAlways:
                        taskCompletionSource.SetResult(locationManager);
                        break;
                    default:
                        throw new AuthorizationException(
                            StringResources.GPSAuthorizationException);
                }
            };

            if (UIDevice.CurrentDevice.CheckSystemVersion(8, 0))
            {
                locationManager.RequestAlwaysAuthorization(); 
            }
            if (UIDevice.CurrentDevice.CheckSystemVersion(9, 0))
            {
                locationManager.AllowsBackgroundLocationUpdates = true;
            }
            return taskCompletionSource.Task;
#else
            throw new Exception();
#endif
        }
    }
}
#endif
