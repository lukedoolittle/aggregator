#if __MOBILE__
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Foundations.Serialization;
using Material.Contracts;
using Material.Exceptions;
using Newtonsoft.Json.Linq;
using Plugin.Geolocator.Abstractions;

namespace Material.Adapters
{
    public class GPSAdapter : IGPSAdapter
    {
        private readonly IGeolocator _geolocator;

        public GPSAdapter(IGeolocator geolocator)
        {
            _geolocator = geolocator;
        }

        public async Task<IEnumerable<Tuple<DateTimeOffset, JObject>>> GetPosition()
        {
            if (!_geolocator.IsGeolocationEnabled)
            {
                throw new NoConnectivityException(
                    StringResources.GPSDisabledConnectivityException);
            }

            try
            {
                var position = await _geolocator
                    .GetPositionAsync(10000)
                    .ConfigureAwait(false);
            
                return new List<Tuple<DateTimeOffset, JObject>>
                {
                    new Tuple<DateTimeOffset, JObject>(
                        position.Timestamp, 
                        position.AsJObject())
                };
            }
            catch (Exception)
            {
                throw new NoConnectivityException(
                    StringResources.GPSTimeoutConnectivityException);
            }
        }
    }
}
#endif