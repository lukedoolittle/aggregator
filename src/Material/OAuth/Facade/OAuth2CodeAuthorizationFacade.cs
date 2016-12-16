using System;
using System.Threading.Tasks;
using Foundations.HttpClient.Authenticators;
using Material.Contracts;
using Material.Infrastructure;
using Material.Infrastructure.Credentials;
using Material.OAuth.AuthenticatorParameters;

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

            var builder = new AuthenticatorBuilder()
                .AddParameter(new OAuth2ClientId(ClientId))
                .AddParameter(new OAuth2ClientSecret(_clientSecret))
                .AddParameter(new OAuth2CallbackUri(CallbackUri))
                .AddParameter(new OAuth2Code(intermediateCredentials.Code))
                .AddParameter(new OAuth2Scope(ResourceProvider.Scope));

            return OAuth.GetAccessToken(
                ResourceProvider.TokenUrl,
                builder,
                ResourceProvider.Headers);
        }
    }
}
