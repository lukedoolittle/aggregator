using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using Aggregator.Framework.Contracts;

namespace Aggregator.Test.Mocks
{
    using System.Threading.Tasks;

    public class ClientMock : IRequestClient
    {
        public Task<IEnumerable<Tuple<DateTimeOffset, JObject>>> GetDataPoints(string recencyValue)
        {
            throw new NotImplementedException();
        }
    }
}
