using System;
using System.Collections.Generic;
using System.Text;
using Aggregator.Framework.Contracts;
using Aggregator.Test.Helpers.Mocks;
using Material.Contracts;

namespace Quantfabric.Test.Application.Mocks
{
    using System.Threading.Tasks;
    using Newtonsoft.Json.Linq;

    public class SMSManagerMock : MockBase<ISMSAdapter>, ISMSAdapter
    {
        Task<IEnumerable<Tuple<DateTimeOffset, JObject>>> ISMSAdapter.GetAllSMS(string filterDate)
        {
            throw new NotImplementedException();
        }
    }
}
