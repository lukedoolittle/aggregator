using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BatmansBelt.Serialization;
using Newtonsoft.Json.Linq;
using Aggregator.Framework.Contracts;
using Aggregator.Framework.Exceptions;
using Plugin.Geolocator.Abstractions;

namespace Aggregator.Infrastructure.ComponentManagers
{
    public class GPSManager : IGPSManager
    {
        private readonly IGeolocator _geolocator;

        public GPSManager(IGeolocator geolocator)
        {
            _geolocator = geolocator;
        }

        public async Task<IEnumerable<Tuple<DateTimeOffset, JObject>>> GetPosition()
        {
            if (!_geolocator.IsGeolocationEnabled)
            {
                throw new ConnectivityException();
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
                throw new ConnectivityException();
            }
        }
    }
}