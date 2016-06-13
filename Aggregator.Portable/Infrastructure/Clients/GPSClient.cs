using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Aggregator.Framework.Contracts;

namespace Aggregator.Infrastructure.Clients
{
    public class GPSClient : IRequestClient
    {
        private readonly IGPSManager _manager;

        public GPSClient(IGPSManager manager)
        {
            _manager = manager;
        }

        public Task<IEnumerable<Tuple<DateTimeOffset, JObject>>> GetDataPoints(
            string recencyValue)
        {
            return _manager.GetPosition();
        }
    }
}