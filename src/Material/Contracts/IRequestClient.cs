using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Material.Contracts
{
    public interface IRequestClient
    {
        Task<IEnumerable<Tuple<DateTimeOffset, JObject>>> GetDataPoints(string recencyValue = null);
    }
}
