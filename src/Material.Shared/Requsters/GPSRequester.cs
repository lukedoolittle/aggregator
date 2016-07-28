#if __MOBILE__
using Material.Adapters;
using Material.Infrastructure.Static;
using Plugin.Geolocator;

namespace Material
{
    using System.Threading.Tasks;
    public class GPSRequester
    { 
        public Task<GPSResponse> MakeGPSRequest()
        {
            return new GPSAdapter(CrossGeolocator.Current).GetPosition();
        }
    }
}
#endif
