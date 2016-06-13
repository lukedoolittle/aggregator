using Newtonsoft.Json.Linq;
using Aggregator.Domain.Write;
using SimpleCQRS.Domain;

namespace Aggregator.Domain.Events
{
    public class TokenCreated<TService> : Event
        where TService : Service
    {
        public JObject Values { get; private set; }

        public TokenCreated(JObject values)
        {
            Values = values;
        }
    }
}
