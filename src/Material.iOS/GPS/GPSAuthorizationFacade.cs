using System.Threading.Tasks;
using CoreLocation;
using UIKit;

namespace Material.GPS
{
    public class GPSAuthorizationFacade
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        public Task<bool> AuthorizeContinuousGPSUsage()
        {
            var taskCompletionSource = new TaskCompletionSource<bool>();
            var locationManager = new CLLocationManager();
            locationManager.AuthorizationChanged += (sender, args) =>
            {
                switch (args.Status)
                {
                    case CLAuthorizationStatus.NotDetermined:
                        break;
                    case CLAuthorizationStatus.AuthorizedAlways:
                        taskCompletionSource.SetResult(true);
                        break;
                    case CLAuthorizationStatus.Restricted:
                        taskCompletionSource.SetResult(true);
                        break;
                    case CLAuthorizationStatus.Denied:
                        taskCompletionSource.SetResult(false);
                        break;
                    case CLAuthorizationStatus.AuthorizedWhenInUse:
                        taskCompletionSource.SetResult(true);
                        break;
                    default:
                        taskCompletionSource.SetResult(false);
                        break;
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
