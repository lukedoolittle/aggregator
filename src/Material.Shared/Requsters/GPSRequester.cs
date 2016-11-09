#if __MOBILE__
using Material.Infrastructure.Requests;
#if __IOS__
using Material.GPS;
#endif
#if __ANDROID__
using Material.Permissions;
#endif

namespace Material
{
    using System.Threading.Tasks;
    public class GPSRequester
    { 
        /// <summary>
        /// Get a single GPS data point from the current platform
        /// </summary>
        /// <returns>GPS information</returns>
        public async Task<GPSResponse> MakeGPSRequestAsync()
        {
#if __IOS__
            var locationManager = await new GPSAuthorizationFacade()
                .AuthorizeContinuousGPSUsage()
                .ConfigureAwait(false);
            return await new iOSGPSAdapter(locationManager)
                .GetPositionAsync()
                .ConfigureAwait(false);
#elif __ANDROID__
            var authorizationResult = await new DeviceAuthorizationFacade()
                .AuthorizeGPS()
                .ConfigureAwait(false);
            return await new AndroidGPSAdapter()
                .GetPositionAsync()
                .ConfigureAwait(false);
#else
            throw new NotSupportedException();
#endif
        }
    }
}
#endif
