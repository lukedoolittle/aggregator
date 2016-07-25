using Newtonsoft.Json.Linq;
using Aggregator.Domain.Write;
using Material.Infrastructure;
using SimpleCQRS.Domain;

namespace Aggregator.Domain.Events
{
    public class TokenCreated<TService> : Event
        where TService : ResourceProvider
    {
        public JObject Values { get; private set; }

        public TokenCreated(JObject values)
        {
            Values = values;
        }
    }
}
