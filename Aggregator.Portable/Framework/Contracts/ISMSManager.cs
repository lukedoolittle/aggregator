using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace Aggregator.Framework.Contracts
{
    using System.Threading.Tasks;

    public interface ISMSManager
    {
        //Action<IEnumerable<Tuple<DateTimeOffset, JObject>>> Handler { get; set; }

        Task<IEnumerable<Tuple<DateTimeOffset, JObject>>> GetAllSMS(string filterDate);
    }
}
