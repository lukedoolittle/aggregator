using System;
using Aggregator.Framework.Contracts;
using Aggregator.Infrastructure;
using Aggregator.Task.Commands;
using Foundations.Serialization;
using Material.Contracts;
using Material.Infrastructure;
using Material.Infrastructure.Credentials;
using SimpleCQRS.Framework.Contracts;

namespace Aggregator.Task.Authentication
{
    using System.Threading.Tasks;

    public class RefreshTokenTask<TService> : ITask
        where TService : OAuth2ResourceProvider, new()
    {
        private readonly ICommandSender _sender;
        private readonly Guid _aggregateId;
        private readonly IRefreshTokenFacade _facade;

        public RefreshTokenTask(
            ICommandSender sender,
            Guid aggregateId,
            IRefreshTokenFacade facade)
        {
            _aggregateId = aggregateId;
            _facade = facade;
            _sender = sender;
        }
        
        //TODO: if we have this task handle a token changed event from the given service and then subscribe it
        //we could avoid this ugly untyped parameter here
        async Task ITask.Execute(object parameter)
        {
            var expiredCredentials = parameter as OAuth2Credentials;

            if (expiredCredentials == null)
            {
                throw new Exception();
            }

            var accessToken = await _facade
                .GetRefreshedAccessTokenCredentials<TService>(expiredCredentials)
                .ConfigureAwait(false);

            await _sender.Send(
                new UpdateTokenCommand<TService>(
                    _aggregateId, 
                    accessToken.AsJObject(), 
                    0))
                .ConfigureAwait(false);
        }
    }
}
