using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Material.Contracts
{
    public interface ISMSAdapter
    {
        //Action<IEnumerable<Tuple<DateTimeOffset, JObject>>> Handler { get; set; }

        Task<IEnumerable<Tuple<DateTimeOffset, JObject>>> GetAllSMS(string filterDate);
    }
}
