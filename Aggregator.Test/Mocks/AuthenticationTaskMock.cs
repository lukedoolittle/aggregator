using System;
using Aggregator.Domain.Write;
using Aggregator.Framework.Contracts;
using Aggregator.Framework.Enums;
using Aggregator.Infrastructure.Credentials;
using Aggregator.Task.Authentication;
using SimpleCQRS.Framework.Contracts;

namespace Aggregator.Test.Mocks
{
    using System.Threading.Tasks;

    public class AuthenticationTaskMock<TCredentials, TService> : AuthenticationTaskBase<TCredentials, TService>
        where TService : Service, new()
        where TCredentials : TokenCredentials
    {
        protected override ResponseTypeEnum _responseType { get; }

        private TCredentials _credentials;

        public AuthenticationTaskMock(
            Guid aggregateId,
            int originalVersion,
            ICommandSender sender,
            Uri callbackUri,
            IWebAuthorizer authorizer,
            bool isUpdate) : base(
                aggregateId, 
                originalVersion, 
                sender, 
                callbackUri, 
                authorizer, 
                isUpdate)
        { }

        public AuthenticationTaskMock<TCredentials, TService> SetTokenToReturn(
            TCredentials credentials)
        {
            _credentials = credentials;
            return this;
        }

        protected override Task<TCredentials> GetAccessTokenFromResult(
            TCredentials result)
        {
            return Task.FromResult(_credentials);
        }

        protected override Task<Uri> GetAuthorizationPath()
        {
            return Task.FromResult(new Uri("http://google.com"));
        }
    }
}
