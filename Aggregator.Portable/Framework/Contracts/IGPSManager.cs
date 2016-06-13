using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Aggregator.Framework.Contracts
{
    public interface IGPSManager
    {
        Task<IEnumerable<Tuple<DateTimeOffset, JObject>>> GetPosition();
    }
}
