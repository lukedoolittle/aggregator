using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Aggregator.Domain.Write;
using Aggregator.Framework.Contracts;
using Material.Contracts;
using Material.Infrastructure;
using Material.Metadata;

namespace Aggregator.Test.Mocks
{
    [ServiceType(typeof(ResourceProviderMock))]
    public class RequestMock : Request
    {
        public static string Payload { get; set; }
        public override string PayloadProperty => Payload;
        public override TimestampOptions ResponseTimestamp { get; }
    }

    public class ClientMock : IRequestClient
    {
        public Task<IEnumerable<Tuple<DateTimeOffset, JObject>>> GetDataPoints(string recencyValue)
        {
            throw new NotImplementedException();
        }
    }
}
