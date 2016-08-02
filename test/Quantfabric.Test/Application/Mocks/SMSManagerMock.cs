using System;
using System.Collections.Generic;
using Aggregator.Test.Helpers.Mocks;
using Material.Contracts;
using Material.Infrastructure.Static;

namespace Quantfabric.Test.Application.Mocks
{
    using System.Threading.Tasks;
    using Newtonsoft.Json.Linq;

    public class SMSManagerMock : MockBase<ISMSAdapter>, ISMSAdapter
    {
        public Task<IEnumerable<SMSMessage>> GetAllSMS(DateTime filterDate)
        {
            throw new NotImplementedException();
        }
    }
}
