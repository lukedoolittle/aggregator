using System;
using Material.Contracts;
using Aggregator.Framework.Contracts;
using Aggregator.Task.Commands;
using Foundations.Serialization;
using Material.Infrastructure;
using Material.Infrastructure.Credentials;
using SimpleCQRS.Framework.Contracts;

namespace Aggregator.Task.Authentication
{
    using System.Threading.Tasks;

    public class OAuthAuthenticationTask<TCredentials, TService> : ITask
        where TService : ResourceProvider, new()
        where TCredentials : TokenCredentials
    {
        private readonly Guid _aggregateId;
        private readonly int _originalVersion;
        private readonly ICommandSender _sender;
        private readonly bool _isUpdate;
        private readonly IOAuthAuthenticationTemplate<TCredentials> _oauthAuthenticationTemplate;

        public OAuthAuthenticationTask(
            Guid aggregateId, 
            int originalVersion, 
            ICommandSender sender,
            IOAuthAuthenticationTemplate<TCredentials> oauthAuthenticationTemplate,
            bool isUpdate)
        {
            _aggregateId = aggregateId;
            _originalVersion = originalVersion;
            _sender = sender;
            _isUpdate = isUpdate;
            _oauthAuthenticationTemplate = oauthAuthenticationTemplate;
        }

        async Task ITask.Execute(object parameter)
        {
            var accessToken = await _oauthAuthenticationTemplate
                .GetAccessTokenCredentials(_aggregateId.ToString())
                .ConfigureAwait(false);

            //This may seem odd to change the credentials back to a JObject
            //but we want commands/events to consist of as little language 
            //specific types as possible for distributed eventing across 
            //process boundaries 

            if (_isUpdate)
            {
                await _sender.Send(
                    new UpdateTokenCommand<TService>(
                        _aggregateId,
                        accessToken.AsJObject(),
                        _originalVersion))
                    .ConfigureAwait(false);
            }
            else
            {
                await _sender.Send(
                    new CreateTokenCommand<TService>(
                        _aggregateId,
                        accessToken.AsJObject(),
                        _originalVersion))
                    .ConfigureAwait(false);
            }
        }
    }
}
