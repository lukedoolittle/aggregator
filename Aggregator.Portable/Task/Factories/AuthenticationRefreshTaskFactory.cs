using System;
using BatmansBelt.Extensions;
using BatmansBelt.Reflection;
using Aggregator.Framework.Contracts;
using Aggregator.Infrastructure;
using Aggregator.Task.Authentication;
using SimpleCQRS.Framework.Contracts;

namespace Aggregator.Task.Factories
{
    public class AuthenticationRefreshTaskFactory : IAuthenticationRefreshTaskFactory
    {
        private readonly ICommandSender _sender;
        private readonly IOAuthFactory _oauthFactory;

        public AuthenticationRefreshTaskFactory(
            ICommandSender sender, 
            IOAuthFactory oauthFactory)
        {
            _sender = sender;
            _oauthFactory = oauthFactory;
        }

        public ITask GenerateRefreshTask(
            Type serviceType,
            Guid aggregateId)
        {
            if (serviceType.HasBase(typeof(OAuth2Service)))
            {
                return new InstanceCreator(typeof(RefreshTokenTask<>))
                    .AddGenericParameter(serviceType)
                    .AddConstructorParameter(_sender)
                    .AddConstructorParameter(aggregateId)
                    .AddConstructorParameter(_oauthFactory.GetOAuth2())
                    .Create<ITask>();
            }
            else
            {
                return null;
            }
        }
    }
}
