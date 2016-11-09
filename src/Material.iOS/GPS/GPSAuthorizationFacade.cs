using System.Threading.Tasks;
using CoreLocation;
using Material.Exceptions;
using UIKit;

namespace Material.GPS
{
    public class GPSAuthorizationFacade
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        public Task<CLLocationManager> AuthorizeContinuousGPSUsage()
        {
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
        }
    }
}
