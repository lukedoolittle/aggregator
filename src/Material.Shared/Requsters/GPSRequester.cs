#if __MOBILE__
using Material.Adapters;
using Material.Infrastructure.Static;
#if __IOS__
using Plugin.Geolocator;
#endif

namespace Material
{
    using System.Threading.Tasks;
    public class GPSRequester
    { 
        public Task<GPSResponse> MakeGPSRequestAsync()
        {
#if __IOS__
            return new GPSAdapter(CrossGeolocator.Current).GetPositionAsync();
#elif __ANDROID__
            return new AndroidGPSAdapter().GetPositionAsync();
#else
            throw new NotSupportedException();
#endif

        }
    }
}
#endif
