using System.IO;
using Aggregator.Domain.Write;
using Foundations.Serialization;
using Material.Infrastructure;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Aggregator.Test
{
    public static class TestHelpers
    {
        private static readonly string _pathToTestCredentials =
            "../../../Quantfabric.Test/testCredentials.json";

        public static void WriteCredentials<TService>(object token)
            where TService : ResourceProvider
        {
            var serviceName = typeof(TService).Name;
            var credentials2 = File.ReadAllText(_pathToTestCredentials).AsEntity<JObject>();
            credentials2[serviceName] = token.AsJObject();
            var json = JsonConvert.SerializeObject(credentials2, Formatting.Indented);
            File.WriteAllText(_pathToTestCredentials, json);
        }
    }
}
