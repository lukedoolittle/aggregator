using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Aggregator.Framework.Contracts;
using Aggregator.Test.Helpers.Mocks;
using Material.Contracts;
using Newtonsoft.Json.Linq;

namespace Quantfabric.Test.Application.Mocks
{
    public class IGPSAdapterMock : MockBase<IGPSAdapter>, IGPSAdapter
    {
        Task<IEnumerable<Tuple<DateTimeOffset, JObject>>> IGPSAdapter.GetPosition()
        {
            throw new NotImplementedException();
        }
    }
}
