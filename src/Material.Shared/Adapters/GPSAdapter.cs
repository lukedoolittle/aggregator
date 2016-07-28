#if __MOBILE__
using System;
using System.Threading.Tasks;
using Material.Contracts;
using Material.Exceptions;
using Material.Infrastructure.Static;
using Plugin.Geolocator.Abstractions;

namespace Material.Adapters
{
    public class GPSAdapter : IGPSAdapter
    {
        private const int GPS_TIMEOUT_IN_MS = 10000;
        private readonly IGeolocator _geolocator;

        public GPSAdapter(IGeolocator geolocator)
        {
            _geolocator = geolocator;
        }

        public async Task<GPSResponse> GetPosition()
        {
            if (!_geolocator.IsGeolocationEnabled)
            {
                throw new NoConnectivityException(
                    StringResources.GPSDisabledConnectivityException);
            }

            try
            {
                var position = await _geolocator
                    .GetPositionAsync(GPS_TIMEOUT_IN_MS)
                    .ConfigureAwait(false);
            
                return new GPSResponse
                {
                    Accuracy = position.Accuracy,
                    AltitudeAccuracy = position.AltitudeAccuracy,
                    Altitude = position.Altitude,
                    Heading = position.Heading,
                    Latitude = position.Latitude,
                    Longitude = position.Longitude,
                    Speed = position.Speed,
                    Timestamp = position.Timestamp
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