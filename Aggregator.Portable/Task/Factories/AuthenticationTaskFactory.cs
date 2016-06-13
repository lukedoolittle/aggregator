using System;
using Aggregator.Framework.Contracts;
using Aggregator.Framework.Enums;
using Aggregator.Infrastructure;
using Aggregator.Infrastructure.Credentials;
using Aggregator.Task.Authentication;
using SimpleCQRS.Framework.Contracts;
using Service = Aggregator.Domain.Write.Service;

namespace Aggregator.Task.Factories
{
    public abstract class AuthenticationTaskFactory
    {
        private readonly IWebAuthorizerFactory _webAuthorizerFactory;
        private readonly IBluetoothAuthorizerFactory _bluetoothAuthorizerFactory;
        private readonly ICommandSender _sender;
        private readonly IClientCredentials _clientCredentials;
        private readonly IOAuthFactory _oauthFactory;

        protected AuthenticationTaskFactory(
            IBluetoothAuthorizerFactory bluetoothAuthorizerFactory,
            IWebAuthorizerFactory webAuthorizerFactory,
            IClientCredentials clientCredentials,
            ICommandSender sender,
            IOAuthFactory oauthFactory)
        {
            _webAuthorizerFactory = webAuthorizerFactory;
            _bluetoothAuthorizerFactory = bluetoothAuthorizerFactory;
            _clientCredentials = clientCredentials;
            _sender = sender;
            _oauthFactory = oauthFactory;
        }

        protected AuthenticationTaskFactory(
            IWebAuthorizerFactory webAuthorizerFactory,
            IClientCredentials clientCredentials,
            ICommandSender sender, 
            IOAuthFactory oauthFactory)
        {
            _webAuthorizerFactory = webAuthorizerFactory;
            _clientCredentials = clientCredentials;
            _sender = sender;
            _oauthFactory = oauthFactory;
        }

        protected ITask GenerateTask<TService>(
            Guid aggregateId, 
            int originalVersion,
            object context,
            AuthenticationInterfaceEnum browserType,
            bool tokenAlreadyExists = false) 
            where TService : Service, new()
        {
            var service = new TService();

            if (service is BluetoothService)
            {
                return new BluetoothAuthenticationTask<TService>(
                    _bluetoothAuthorizerFactory.GetAuthorizer(context),
                    _sender,
                    aggregateId,
                    originalVersion,
                    tokenAlreadyExists);
            }
            if (service is OAuth1Service)
            {
                return new OAuth1AuthenticationTask<TService>(
                    service as OAuth1Service,
                    _clientCredentials.GetClientCredentials<TService, OAuth1Credentials>(),
                    _oauthFactory.GetOAuth1(),
                    _webAuthorizerFactory.GetAuthorizer<TService>(context, browserType),
                    _sender,
                    aggregateId,
                    originalVersion,
                    tokenAlreadyExists);
            }
            if (service is OAuth2Service)
            {
                return new OAuth2AuthenticationTask<TService>(
                    service as OAuth2Service,
                    _clientCredentials.GetClientCredentials<TService, OAuth2Credentials>(), 
                    _oauthFactory.GetOAuth2(),
                    _webAuthorizerFactory.GetAuthorizer<TService>(context, browserType),
                    _sender,
                    aggregateId,
                    originalVersion,
                    tokenAlreadyExists);
            }

            throw new NotSupportedException();
        }
    }
}
