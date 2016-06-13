using System;
using BatmansBelt.Serialization;
using Aggregator.Domain.Write;
using Aggregator.Framework.Contracts;
using Aggregator.Infrastructure;
using Aggregator.Infrastructure.Credentials;
using Aggregator.Task.Commands;
using SimpleCQRS.Framework.Contracts;

namespace Aggregator.Task.Authentication
{
    using System.Threading.Tasks;

    public class RefreshTokenTask<TService> : ITask
        where TService : Service, new()
    {
        private readonly ICommandSender _sender;
        private readonly Guid _aggregateId;
        private readonly IOAuth2 _oauth;

        public RefreshTokenTask(
            ICommandSender sender,
            Guid aggregateId, 
            IOAuth2 oauth)
        {
            _aggregateId = aggregateId;
            _sender = sender;
            _oauth = oauth;
        }

        public async Task Execute(object parameter)
        {
            var expiredCredentials = parameter as OAuth2Credentials;

            if (expiredCredentials == null)
            {
                throw new Exception();
            }

            var accessToken = await GetRefreshedAccessTokenCredentials(expiredCredentials)
                .ConfigureAwait(false);

            await _sender.Send(
                new UpdateTokenCommand<TService>(
                    _aggregateId, 
                    accessToken.AsJObject(), 
                    0))
                .ConfigureAwait(false);
        }

        public async Task<OAuth2Credentials> GetRefreshedAccessTokenCredentials(
            OAuth2Credentials expiredCredentials)
        {
            var service = new TService() as OAuth2Service;

            var token = await _oauth.GetRefreshToken(
                service.AccessUrl,
                expiredCredentials.ClientId,
                expiredCredentials.ClientSecret,
                expiredCredentials.RefreshToken,
                service.HasBasicAuthorization)
                .ConfigureAwait(false);

            return token
                .SetTokenName(expiredCredentials.TokenName)
                .SetClientProperties(
                    expiredCredentials.ClientId,
                    expiredCredentials.ClientSecret)
                .TransferRefreshToken(expiredCredentials.RefreshToken);
        } 
    }
}
