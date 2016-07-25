using System;
using Aggregator.Framework.Contracts;
using Aggregator.Task.Authentication;
using Aggregator.Task.Commands;
using Aggregator.Test.Helpers.Mocks;
using Aggregator.Test.Mocks;
using Aggregator.Test.Helpers;
using Material.Enums;
using Material.Infrastructure.Credentials;
using Material.Infrastructure.ProtectedResources;
using Quantfabric.Test.Application.Mocks;
using Xunit;

namespace Aggregator.Test.Integration.Tasks
{
    public class AuthenticationTaskTests
    {
        [Fact]
        public void ExecuteAuthenticationTaskPublishesReceivedToken()
        {
            //TODO: this test will fail
            var intermediateCredentials = new OAuth2Credentials().SetTokenName(Guid.NewGuid().ToString());
            var finalCredentials = new OAuth2Credentials().SetTokenName(Guid.NewGuid().ToString());
            var senderMock = new CommandSenderMock();
            var authorizerMock = new OAuthAuthenticationTemplateMock<OAuth2Credentials>();
            var aggregateId = Guid.NewGuid();
            var version = 0;

            var task = new OAuthAuthenticationTask<OAuth2Credentials, Facebook>(
                aggregateId,
                version,
                senderMock,
                authorizerMock,
                false);

            ((ITask) task).Execute().Wait();

            var command = senderMock.GetLastCommand<CreateTokenCommand<Facebook>>();
            var newToken = command.Values.ToObject<OAuth2Credentials>();

            Assert.Equal(finalCredentials.TokenName, newToken.TokenName);
        }

        [Fact]
        public void ExecuteAuthenticationWithTokenFlowTaskPublishesIntermediateToken()
        {
            //TODO: this test will fail
            var intermediateCredentials = new OAuth2Credentials().SetTokenName(Guid.NewGuid().ToString());
            var finalCredentials = new OAuth2Credentials().SetTokenName(Guid.NewGuid().ToString());
            var senderMock = new CommandSenderMock();
            var providerMock = new OAuthProviderMock<OAuth2Credentials>()
                .SetResponseType(ResponseTypeEnum.Token)
                .SetAccessTokenResult(finalCredentials);
            var authorizerMock = new OAuthAuthenticationTemplateMock<OAuth2Credentials>();
            var aggregateId = Guid.NewGuid();
            var version = 0;

            var task = new OAuthAuthenticationTask<OAuth2Credentials, Facebook>(
                aggregateId,
                version,
                senderMock,
                authorizerMock,
                false);

            ((ITask)task).Execute().Wait();

            var command = senderMock.GetLastCommand<CreateTokenCommand<Facebook>>();
            var newToken = command.Values.ToObject<OAuth2Credentials>();

            Assert.Equal(intermediateCredentials.TokenName, newToken.TokenName);
        }
    }
}
