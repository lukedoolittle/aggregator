using System;
using Newtonsoft.Json.Linq;
using Application.Configuration;
using Foundations.Extensions;
using Foundations.Serialization;
using Material.Infrastructure;
using Material.Infrastructure.Credentials;
using System.IO;
using Newtonsoft.Json;
#if !__MOBILE__
using System.Configuration;
#endif
namespace Quantfabric.Test.Material
{
    public static class TestSettings
    {
        private const string SETTINGS_FILE_NAME = "Quantfabric.Test.testCredentials.json";

        private static JObject AppSettings => ManifestResource
            .GetResourceAsObject<JObject>(
                SETTINGS_FILE_NAME,
                typeof(TestSettings).Assembly);

        public static readonly bool ShouldPersistCredentials = true;

        public static TToken GetToken<TService, TToken>()
            where TService: ResourceProvider
            where TToken: TokenCredentials
        {
            var token1 = AppSettings[typeof(TService).Name]?.ToString();
            var token = token1.AsEntity<TToken>();

            if (token is OAuth1Credentials)
            {
                var clientCredentials = new CredentialApplicationSettings()
                    .GetClientCredentials<TService, OAuth1Credentials>();
                (token as OAuth1Credentials).SetConsumerProperties(
                    clientCredentials.ConsumerKey,
                    clientCredentials.ConsumerSecret);
            }
            else if (token is OAuth2Credentials)
            {
                var clientCredentials = new CredentialApplicationSettings()
                    .GetClientCredentials<TService, OAuth2Credentials>();

                (token as OAuth2Credentials).SetClientProperties(
                    clientCredentials.ClientId,
                    clientCredentials.ClientSecret);
            }

            return token;
        }

        public static JToken GetToken<TService>()
            where TService : ResourceProvider
        {
            if (typeof (TService).HasBase(typeof (OAuth2ResourceProvider)))
            {
                return GetToken<TService, OAuth2Credentials>().AsJObject();
            }
            else if (typeof (TService).HasBase(typeof (OAuth1ResourceProvider)))
            {
                return GetToken<TService, OAuth1Credentials>().AsJObject();
            }
            else
            {
                throw new Exception();
            }
        }

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
