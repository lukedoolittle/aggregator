﻿using System;
using System.Threading.Tasks;
using Material.OAuth;
using Material.Contracts;
using Material.Enums;
using Material.Infrastructure.Credentials;

namespace Material.Infrastructure.OAuth
{
    public class OAuth1AppBase<TResourceProvider>
        where TResourceProvider : OAuth1ResourceProvider, new()
    {
        private readonly string _consumerKey;
        private readonly string _consumerSecret;
        private readonly string _callbackUrl;
        private readonly IOAuthAuthorizerUIFactory _uiFactory;
        private readonly AuthenticationInterfaceEnum _browserType;

        public OAuth1AppBase(
            string consumerKey,
            string consumerSecret,
            string callbackUrl,
            IOAuthAuthorizerUIFactory uiFactory,
            AuthenticationInterfaceEnum browserType = AuthenticationInterfaceEnum.Embedded)
        {
            _consumerKey = consumerKey;
            _consumerSecret = consumerSecret;
            _callbackUrl = callbackUrl;
            _uiFactory = uiFactory;
            _browserType = browserType;
        }

        public virtual Task<OAuth1Credentials> GetCredentialsAsync()
        {
            var userId = Guid.NewGuid().ToString();

            var securityStrategy = new OAuthSecurityStrategy(
                new InMemoryCryptographicParameterRepository(),
                TimeSpan.FromMinutes(2));

            var builder =
                new OAuthBuilder(
                    _uiFactory,
                    null,
                    securityStrategy);
            var facade = builder.BuildOAuth1Facade(
                new TResourceProvider(),
                new OAuth1Authentication(), 
                _consumerKey,
                _consumerSecret, 
                _callbackUrl);
            var template = builder.BuildOAuth1Template<TResourceProvider>(
                facade,
                _browserType,
                userId);

            return template.GetAccessTokenCredentials(userId);
        }
    }
}
