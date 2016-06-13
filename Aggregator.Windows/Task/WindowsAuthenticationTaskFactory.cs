using System;
using Aggregator.Domain.Write;
using Aggregator.Framework.Contracts;
using Aggregator.Framework.Enums;
using Aggregator.Infrastructure;
using Aggregator.Task.Authentication;
using Aggregator.Task.Factories;
using SimpleCQRS.Framework.Contracts;

namespace Aggregator.Task
{
    public class WindowsAuthenticationTaskFactory : AuthenticationTaskFactory
    {
        public WindowsAuthenticationTaskFactory(
            IWebAuthorizerFactory webAuthorizerFactory,
            IClientCredentials clientCredentials,
            ICommandSender sender,
            IOAuthFactory oauthFactory) : 
            base(webAuthorizerFactory, 
                clientCredentials, 
                sender, 
                oauthFactory)
        {
        }

        public OAuth1AuthenticationTask<TService> GenerateOAuth1Task<TService>()
            where TService : OAuth1Service, new()
        {
            return (OAuth1AuthenticationTask<TService>) GenerateTask<TService>(Guid.Empty, -1);
        }

        public OAuth2AuthenticationTask<TService> GenerateOAuth2Task<TService>()
            where TService : OAuth2Service, new()
        {
            return (OAuth2AuthenticationTask<TService>)GenerateTask<TService>(Guid.Empty, -1);
        }

        public ITask GenerateTask<TService>(
            Guid aggregateId,
            int originalVersion,
            bool tokenAlreadyExists = false)
            where TService : Service, new()
        {
            return GenerateTask<TService>(
                aggregateId, 
                originalVersion, 
                null, 
                AuthenticationInterfaceEnum.Dedicated, 
                tokenAlreadyExists);
        }
    }
}
