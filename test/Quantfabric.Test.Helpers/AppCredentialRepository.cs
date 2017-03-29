#if !__FOUNDATIONS__
using System;
using Material.Domain.Credentials;
using Application.Configuration;
using Material.Contracts;
using Material.Domain.Core;
using Material.Framework.Extensions;

namespace Quantfabric.Test.Helpers
{
    public class AppCredentialRepository
    {
        private readonly CallbackType _callbackType;
        private readonly IClientCredentials _clientCredentials =
            new CredentialApplicationSettings();

        public AppCredentialRepository(
            CallbackType callbackType)
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
                .GetJsonWebTokenCredentials<TService>()
                .ClientId;
        }

        public string GetPrivateKey<TService>()
            where TService : OAuth2ResourceProvider
        {
            return _clientCredentials
                .GetJsonWebTokenCredentials<TService>()
                .PrivateKey;
        }

        public string GetClientEmail<TService>()
            where TService : OAuth2ResourceProvider
        {
            return _clientCredentials
                .GetJsonWebTokenCredentials<TService>()
                .ClientEmail;
        }

        public ApiKeyCredentials GetApiKeyCredentials<TService>()
            where TService : ApiKeyResourceProvider
        {
            return _clientCredentials.GetApiKeyCredentials<TService>();
        }

        public string GetUsername<TService>()
            where TService : PasswordResourceProvider
        {
            return _clientCredentials.GetPasswordCredentials<TService>().Username;
        }

        public string GetPassword<TService>()
            where TService : PasswordResourceProvider
        {
            return _clientCredentials.GetPasswordCredentials<TService>().Password;
        }

        public string GetAccountName<TService>()
            where TService : ApiKeyResourceProvider
        {
            return _clientCredentials.GetAccountKeyCredentials<TService>().AccountName;
        }

        public string GetAccountKey<TService>()
             where TService : ApiKeyResourceProvider
        {
            return _clientCredentials.GetAccountKeyCredentials<TService>().AccountKey;
        }
    }
}
#endif