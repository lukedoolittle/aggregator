using System;
using System.Threading.Tasks;
using Aggregator.Configuration;
using Aggregator.Domain.Write;
using Aggregator.Infrastructure.Credentials;
using Aggregator.Task;
using Aggregator.Task.Authentication;
using Aggregator.Task.Factories;
using SimpleCQRS.Infrastructure;

namespace Aggregator.Test
{
    public class OAuthTokenFixture : IDisposable
    {
        private readonly WindowsAuthenticationTaskFactory _factory;

        public OAuthTokenFixture()
        {
            var serviceLocator = new Bootstrapper().Run();

            var bus = serviceLocator.GetInstance<MessageBus>();
            var credentials = new CredentialApplicationSettings();
            var authorizerFactory = new WebAuthorizerFactory();
            var oauthFactory = new OAuthFactory();

            _factory = new WindowsAuthenticationTaskFactory(
                    authorizerFactory,
                    credentials,
                    bus,
                    oauthFactory);
        }

        public void Dispose()
        {
        }

        public async Task<TCredentials> CreateToken<TService, TCredentials>(
            Guid aggregateId = default(Guid),
            int originalVersion = -1)
            where TService : Service, new()
            where TCredentials : TokenCredentials
        {
            var task = _factory.GenerateTask<TService>(
                aggregateId,
                originalVersion);

            var token = await (task as AuthenticationTaskBase<TCredentials, TService>)
                .GetAccessTokenCredentials()
                .ConfigureAwait(false);

            return token;
        }
    }
}
