using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using Aggregator.Framework.Contracts;

namespace Aggregator.Infrastructure.Clients
{
    using System.Threading.Tasks;

    public class SMSClient : IRequestClient
    {
        private readonly ISMSManager _manager;

        public SMSClient(ISMSManager manager)
        {
            _manager = manager;
        }

        public Task<IEnumerable<Tuple<DateTimeOffset, JObject>>> GetDataPoints(
            string recencyValue)
        {
            return _manager.GetAllSMS(recencyValue);
        }
    }
}