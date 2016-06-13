using BatmansBelt;
using BatmansBelt.Serialization;
using Newtonsoft.Json;

namespace Aggregator.Configuration
{
    public class SerializationStartupTask : IStartupTask
    {
        public void Execute()
        {
            var contractResolver = new PrivateMembersContractResolver();
            JsonConvert.DefaultSettings = () => new JsonSerializerSettings
            {
                MissingMemberHandling = MissingMemberHandling.Ignore,
                DefaultValueHandling = DefaultValueHandling.Ignore,
                ContractResolver = contractResolver,
                TypeNameHandling = TypeNameHandling.All,
                DateFormatHandling = DateFormatHandling.IsoDateFormat,
                DateParseHandling = DateParseHandling.None,
                DateTimeZoneHandling = DateTimeZoneHandling.Unspecified,
            };
        }
    }
}
