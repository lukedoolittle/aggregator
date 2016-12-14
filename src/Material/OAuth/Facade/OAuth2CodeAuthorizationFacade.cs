using System;
using System.Threading.Tasks;
using Material.Contracts;
using Material.Infrastructure;
using Material.Infrastructure.Credentials;

namespace Material.OAuth.Facade
{
    public class OAuth2CodeAuthorizationFacade : OAuth2AuthorizationFacadeBase
    {
        private readonly string _clientSecret;

        public OAuth2CodeAuthorizationFacade(
            OAuth2ResourceProvider resourceProvider,
            string clientId,
            string clientSecret,
            Uri callbackUri,
            IOAuth2AuthorizationAdapter oauth,
            IOAuthSecurityStrategy strategy) :
                base(
                resourceProvider,
                clientId,
                callbackUri,
                oauth,
                strategy)
        {
            _clientSecret = clientSecret;
        }

        protected override Task<OAuth2Credentials> GetRawAccessToken(
            OAuth2Credentials intermediateCredentials,
            string userId)
        {
            if (intermediateCredentials == null) throw new ArgumentNullException(nameof(intermediateCredentials));

            ResourceProvider.SetClientProperties(
                ClientId,
                _clientSecret);

            return OAuth.GetAccessToken(
                ResourceProvider.TokenUrl,
                ClientId,
                _clientSecret,
                null,
                CallbackUri,
                intermediateCredentials.Code,
                ResourceProvider.Scope,
                ResourceProvider.Headers);
        }
    }
}
