using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Aggregator.Framework.Contracts;
using Material.Contracts;

namespace Aggregator.Infrastructure.Clients
{
    public class GPSClient : IRequestClient
    {
        private readonly IGPSAdapter _manager;

        public GPSClient(IGPSAdapter manager)
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