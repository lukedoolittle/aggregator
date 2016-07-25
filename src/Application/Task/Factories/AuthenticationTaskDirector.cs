using System;
using Material.OAuth;
using Aggregator.Domain.Write;
using Aggregator.Framework.Contracts;
using Aggregator.Infrastructure;
using Aggregator.Task.Authentication;
using Material.Contracts;
using Material.Enums;
using Material.Infrastructure;
using Material.Infrastructure.Credentials;
using SimpleCQRS.Framework.Contracts;

namespace Aggregator.Task.Factories
{
    public class AuthenticationTaskDirector
    {
        private readonly IBluetoothAuthorizerUI _bluetoothAuthorizerUI;
        private readonly ICommandSender _sender;
        private readonly OAuthBuilder _builder;

        public AuthenticationTaskDirector(
            IBluetoothAuthorizerUI bluetoothAuthorizerUI,
            ICommandSender sender,
            OAuthBuilder builder)
        {
            _bluetoothAuthorizerUI = bluetoothAuthorizerUI;
            _sender = sender;
            _builder = builder;
        }

        public AuthenticationTaskDirector(
            ICommandSender sender, 
            OAuthBuilder builder)
        {
            _sender = sender;
            _builder = builder;
        }

        public ITask GenerateTask<TResourceProvider>(
            Guid aggregateId, 
            int originalVersion,
            AuthenticationInterfaceEnum browserType = 
                AuthenticationInterfaceEnum.Dedicated,
            bool tokenAlreadyExists = false) 
            where TResourceProvider : ResourceProvider, new()
        {
            var resourceProvider = new TResourceProvider();

            if (resourceProvider is BluetoothResourceProvider)
            {
                return new BluetoothAuthenticationTask<TResourceProvider>(
                    _bluetoothAuthorizerUI,
                    _sender,
                    aggregateId,
                    originalVersion,
                    tokenAlreadyExists);
            }
            if (resourceProvider is OAuth1ResourceProvider)
            {
                var provider = resourceProvider as OAuth1ResourceProvider;
                var credentials = _builder
                    .BuildCredentials<TResourceProvider, OAuth1Credentials>();

                var authentication = _builder.BuildOAuth1Facade(
                    provider, 
                    credentials.ConsumerKey, 
                    credentials.ConsumerSecret,
                    credentials.CallbackUrl);

                var template = _builder.BuildOAuth1Template<TResourceProvider>(
                    authentication, 
                    browserType);

                return new OAuthAuthenticationTask<OAuth1Credentials, TResourceProvider>(
                    aggregateId,
                    originalVersion,
                    _sender,
                    template,
                    tokenAlreadyExists);
            }
            if (resourceProvider is OAuth2ResourceProvider)
            {
                var userId = aggregateId.ToString();
                var provider = resourceProvider as OAuth2ResourceProvider;
                var credentials = _builder
                    .BuildCredentials<TResourceProvider, OAuth2Credentials>();

                var authentication = _builder.BuildOAuth2Facade(
                    provider,
                    credentials.ClientId,
                    credentials.ClientSecret,
                    userId,
                    credentials.CallbackUrl);

                OAuthAuthenticationTemplate<OAuth2Credentials> template = null;
                switch (provider.Flow)
                {
                    case ResponseTypeEnum.Token:
                        template = _builder.BuildOAuth2TokenTemplate<TResourceProvider>(
                            authentication,
                            browserType,
                            userId);
                        break;
                    case ResponseTypeEnum.Code:
                        template = _builder.BuildOAuth2CodeTemplate<TResourceProvider>(
                            authentication,
                            browserType,
                            userId);
                        break;
                    default:
                        throw new NotSupportedException();
                }

                return new OAuthAuthenticationTask<OAuth2Credentials, TResourceProvider>(
                    aggregateId,
                    originalVersion,
                    _sender,
                    template,
                    tokenAlreadyExists);
            }

            throw new NotSupportedException();
        }
    }
}
