using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Aggregator.Domain.Write;
using Aggregator.Framework.Contracts;
using Aggregator.Infrastructure;
using Aggregator.Infrastructure.Authentication;
using Aggregator.Infrastructure.Credentials;
using Aggregator.Task.Authentication;
using Aggregator.Test.Fixtures;
using Aggregator.Test.Helpers.Mocks;
using Service = Aggregator.Domain.Write.Service;

namespace Aggregator.Test.Helpers.Fixtures
{
    public class OAuthRequestFixture : IDisposable
    {
        public DateTime MinimumSampleDatetime { get; }
        public DateTime MaximumSampleDatetime { get; }

        private readonly IClientFactory _factory;

        public OAuthRequestFixture()
        {
            MinimumSampleDatetime = DateTime.Now.Subtract(TimeSpan.FromDays(365*10));
            MaximumSampleDatetime = DateTime.Now;

            TestHelpers.SetUpSerialization();

            _factory = TestHelpers.CreateClientFactory();
        }

#if __MOBILE__
        public async Task<IEnumerable<Tuple<DateTimeOffset, JObject>>> MakeRequestForOnboardService<TRequest>()
            where TRequest : Request, new()
        {
            var client = _factory.CreateClient(typeof(TRequest));
            return await client.GetDataPoints("").ConfigureAwait(false);
        }
#endif

        public async Task<IEnumerable<Tuple<DateTimeOffset, JObject>>> MakeRequestForService<TService, TRequest>(
                string recency = "")
            where TService : Service, new()
            where TRequest : Request, new()
        {
            TokenCredentials credentials = null;
            var baseServiceType = typeof (TService).BaseType;

            if (baseServiceType == typeof(OAuth1Service))
            {
                credentials = TestSettings.GetToken<TService, OAuth1Credentials>();
            }
            else if (baseServiceType == typeof(OAuth2Service))
            {
                var oauth2Credentials = TestSettings.GetToken<TService, OAuth2Credentials>();
                if (oauth2Credentials.IsTokenExpired)
                {
                    var refreshToken = oauth2Credentials.RefreshToken;
                    oauth2Credentials = await RefreshAToken<TService>(oauth2Credentials);
                    oauth2Credentials.TransferRefreshToken(refreshToken);
                }

                credentials = oauth2Credentials;
            }
            else
            {
                throw new Exception("Service type unrecognized");
            }

            var client = _factory.CreateClient(typeof(TRequest), credentials);
            return await client.GetDataPoints(recency).ConfigureAwait(false);
        }

        private async Task<OAuth2Credentials> RefreshAToken<TService>(
            OAuth2Credentials expiredToken)
            where TService : Service, new()
        {
            var task = new RefreshTokenTask<TService>(
                new CommandSenderMock(),
                Guid.NewGuid(),
                new OAuth2());

            var token = await task
                .GetRefreshedAccessTokenCredentials(expiredToken)
                .ConfigureAwait(false);

            return token;
        }

        public void Dispose()
        {
        }
    }
}
