using System;
using BatmansBelt.Serialization;
using Aggregator.Domain.Write;
using Aggregator.Framework.Contracts;
using Aggregator.Framework.Enums;
using Aggregator.Infrastructure.Credentials;
using Aggregator.Task.Commands;
using SimpleCQRS.Framework.Contracts;

namespace Aggregator.Task.Authentication
{
    using System.Threading.Tasks;

    public abstract class AuthenticationTaskBase<TCredentials, TService> : ITask
        where TService : Service
        where TCredentials : TokenCredentials
    {
        private readonly Guid _aggregateId;
        private readonly int _originalVersion;
        private readonly ICommandSender _sender;
        private readonly IWebAuthorizer _authorizer;
        private readonly bool _isUpdate;
        private readonly Uri _callbackUri;
        protected abstract ResponseTypeEnum _responseType { get; }

        protected AuthenticationTaskBase(
            Guid aggregateId, 
            int originalVersion, 
            ICommandSender sender, 
            Uri callbackUri, 
            IWebAuthorizer authorizer,
            bool isUpdate)
        {
            _aggregateId = aggregateId;
            _originalVersion = originalVersion;
            _sender = sender;
            _callbackUri = callbackUri;
            _authorizer = authorizer;
            _isUpdate = isUpdate;
        }

        async Task ITask.Execute(object parameter = null)
        {
            var accessToken = await GetAccessTokenCredentials()
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

        public async Task<TCredentials> GetAccessTokenCredentials()
        {
            if (_authorizer.BrowserType == AuthenticationInterfaceEnum.Dedicated
                && _responseType == ResponseTypeEnum.Token)
            {
                throw new NotImplementedException();
            }

            var authorizationPath = await GetAuthorizationPath()
                .ConfigureAwait(false);
            var result = await _authorizer.Authorize<TCredentials>(
                    _callbackUri,
                    authorizationPath)
                .ConfigureAwait(false);
            if (_responseType == ResponseTypeEnum.Token)
            {
                return result;
            }
            else
            {
                var accessToken = await GetAccessTokenFromResult(result)
                    .ConfigureAwait(false);

                return accessToken;
            }
        } 

        protected abstract Task<Uri> GetAuthorizationPath();

        protected abstract Task<TCredentials> GetAccessTokenFromResult(TCredentials result);
    }
}
