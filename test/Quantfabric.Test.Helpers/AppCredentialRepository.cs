using System;
using Application.Configuration;
using Foundations.Extensions;
using Material.Contracts;
using Material.Infrastructure;
using Material.Infrastructure.Credentials;

namespace Quantfabric.Test.Helpers
{
    public class AppCredentialRepository
    {
        private readonly CallbackTypeEnum _callbackType;
        private readonly IClientCredentials _clientCredentials =
            new CredentialApplicationSettings();

        public AppCredentialRepository(
            CallbackTypeEnum callbackType)
        {
            _callbackType = callbackType;
        }

        public string GetConsumerKey<TService>()
            where TService : OAuth1ResourceProvider
        {
            var credentials = _clientCredentials
                .GetClientCredentials<TService, OAuth1Credentials>(
                _callbackType);

            return credentials.ConsumerKey;
        }

        public string GetConsumerSecret<TService>()
            where TService : OAuth1ResourceProvider
        {
            var credentials = _clientCredentials
                .GetClientCredentials<TService, OAuth1Credentials>(
                _callbackType);

            return credentials.ConsumerSecret;
        }

        public string GetRedirectUri<TService>()
            where TService : ResourceProvider
        {
            var resourceProviderType = typeof(TService);

            if (resourceProviderType.HasBase(typeof(OAuth1ResourceProvider)))
            {
                var credentials = _clientCredentials
                    .GetClientCredentials<TService, OAuth1Credentials>(
                    _callbackType);

                return credentials.CallbackUrl;
            }
            else if (resourceProviderType.HasBase(typeof(OAuth2ResourceProvider)))
            {
                var credentials = _clientCredentials
                    .GetClientCredentials<TService, OAuth2Credentials>(
                    _callbackType);

                return credentials.CallbackUrl;
            }
            else
            {
                throw new Exception();
            }
        }

        public string GetClientId<TService>()
            where TService : OAuth2ResourceProvider
        {
            var credentials = _clientCredentials
                .GetClientCredentials<TService, OAuth2Credentials>(
                _callbackType);

            return credentials.ClientId;
        }

        public string GetClientSecret<TService>()
            where TService : OAuth2ResourceProvider
        {
            var credentials = _clientCredentials
                .GetClientCredentials<TService, OAuth2Credentials>(
                _callbackType);

            return credentials.ClientSecret;
        }

        public string GetJWTClientId<TService>()
            where TService : OAuth2ResourceProvider
        {
            return _clientCredentials
                .GetJWTCredentials<TService>()
                .ClientId;
        }

        public string GetPrivateKey<TService>()
            where TService : OAuth2ResourceProvider
        {
            return _clientCredentials
                .GetJWTCredentials<TService>()
                .PrivateKey;
        }

        public string GetClientEmail<TService>()
            where TService : OAuth2ResourceProvider
        {
            return _clientCredentials
                .GetJWTCredentials<TService>()
                .ClientEmail;
        }
    }
}
