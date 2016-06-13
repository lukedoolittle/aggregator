using System;
using BatmansBelt;
using BatmansBelt.Extensions;
using BatmansBelt.Serialization;
using Newtonsoft.Json.Linq;
using Aggregator.Configuration;
using Aggregator.Domain.Write;
using Aggregator.Framework.Extensions;
using Aggregator.Framework.Serialization;
using Aggregator.Infrastructure;
using Aggregator.Infrastructure.Credentials;

#if !__MOBILE__
using System.Configuration;
#endif
namespace Aggregator.Test.Helpers
{
    public static class TestSettings
    {
        private const string SETTINGS_FILE_NAME = "Aggregator.Test.testCredentials.json";

        private static JObject AppSettings => ManifestResource
            .GetResourceAsObject<JObject>(
                SETTINGS_FILE_NAME,
                typeof(TestSettings).Assembly);

        public static TToken GetToken<TService, TToken>()
            where TService: Service
            where TToken: TokenCredentials
        {
            var token = AppSettings[typeof(TService).Name]?.ToString().AsEntity<TToken>();

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
            where TService : Service
        {
            if (typeof (TService).HasBase(typeof (OAuth2Service)))
            {
                return GetToken<TService, OAuth2Credentials>().AsJObject();
            }
            else if (typeof (TService).HasBase(typeof (OAuth1Service)))
            {
                return GetToken<TService, OAuth1Credentials>().AsJObject();
            }
            else
            {
                throw new Exception();
            }
        }
    }
}
