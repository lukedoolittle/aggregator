using System;
using Aggregator.Framework.Contracts;
using Aggregator.Infrastructure;
using Aggregator.Task.Authentication;
using Material.Contracts;
using Material.Infrastructure;
using SimpleCQRS.Framework.Contracts;

namespace Aggregator.Task.Factories
{
    public class AuthenticationRefreshTaskFactory : IAuthenticationRefreshTaskFactory
    {
        private readonly ICommandSender _sender;
        private readonly IRefreshTokenFacade _refreshTokenFacade;

        public AuthenticationRefreshTaskFactory(
            ICommandSender sender,
            IRefreshTokenFacade refreshTokenFacade)
        {
            _sender = sender;
            _refreshTokenFacade = refreshTokenFacade;
        }

        public ITask GenerateRefreshTask<TService>(Guid aggregateId) 
            where TService : OAuth2ResourceProvider, new()
        {
            return new RefreshTokenTask<TService>(
                _sender,
                aggregateId, 
                _refreshTokenFacade);
        }
    }
}
