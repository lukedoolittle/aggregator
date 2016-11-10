#if __MOBILE__
using Material.Infrastructure.Responses;
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
        /// <returns>GPS data point</returns>
        public Task<GPSResponse> MakeGPSRequestAsync()
        {
            return MakeGPSRequestAsync(false);
        }

        /// <summary>
        /// Get a single GPS data point from the current platform
        /// </summary>
        /// <returns>GPS information</returns>
        public async Task<GPSResponse> MakeGPSRequestAsync(bool skipGPSAuthorization)
        {
#if __IOS__
            if (!skipGPSAuthorization)
            {
                var locationManager = await new GPSAuthorizationFacade()
                    .AuthorizeContinuousGPSUsage()
                    .ConfigureAwait(false);
            }
            return await new iOSGPSAdapter()
                .GetPositionAsync()
                .ConfigureAwait(false);
#elif __ANDROID__
            if (!skipGPSAuthorization)
            {
                var authorizationResult = await new DeviceAuthorizationFacade()
                    .AuthorizeGPS()
                    .ConfigureAwait(false);
            }
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
