using System;
using System.Threading.Tasks;
using Aggregator.Domain.Write;
using Aggregator.Infrastructure.Authentication;
using Aggregator.Infrastructure.Credentials;
using Aggregator.Task.Authentication;
using Aggregator.Test.Helpers.Mocks;

namespace Aggregator.Test.Fixtures
{
    public class OAuthRefreshTokenFixture : IDisposable
    {
        public OAuthRefreshTokenFixture()
        {
            TestHelpers.SetUpSerialization();
        }

        public async Task<OAuth2Credentials> RefreshAToken<TService>(
            OAuth2Credentials expiredToken)
            where TService : Service, new()
        {
            var task = new RefreshTokenTask<TService>(
                new CommandSenderMock(),
                Guid.NewGuid(),
                new OAuth2());
            var newToken = await task
                .GetRefreshedAccessTokenCredentials(expiredToken)
                .ConfigureAwait(false);

            return newToken;
        }

        public void Dispose()
        {
        }
    }
}
