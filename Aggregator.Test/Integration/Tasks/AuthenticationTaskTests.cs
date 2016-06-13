using System;
using Aggregator.Framework.Contracts;
using Aggregator.Infrastructure.Credentials;
using Aggregator.Infrastructure.Services;
using Aggregator.Task.Commands;
using Aggregator.Test.Helpers.Mocks;
using Aggregator.Test.Mocks;
using Aggregator.Test.Helpers;
using Xunit;

namespace Aggregator.Test.Integration.Tasks
{
    public class AuthenticationTaskTests : IClassFixture<BootstrapFixture>
    {
        [Fact(Skip = "TODO: this test does NOTHING!!! need to rewrite to test actual AuthenticationTask")]
        public async void ExecuteAuthenticationTaskPublishesReceivedToken()
        {
            var credentials = new OAuth2Credentials().SetTokenName(Guid.NewGuid().ToString());
            var senderMock = new CommandSenderMock();
            var authorizerMock = new WebAuthorizerMock().SetReturnToken(credentials);
            var taskMock = new AuthenticationTaskMock<OAuth2Credentials, Facebook>(
                Guid.NewGuid(),
                0,
                senderMock,
                new Uri("http://www.google.com"),
                authorizerMock,
                false)
                .SetTokenToReturn(credentials);

            await (taskMock as ITask).Execute();

            var command = senderMock.GetLastCommand<CreateTokenCommand<Facebook>>();
            var newToken = command.Values.ToObject<OAuth2Credentials>();

            Assert.Equal(credentials.TokenName, newToken.TokenName);
        }
    }
}
