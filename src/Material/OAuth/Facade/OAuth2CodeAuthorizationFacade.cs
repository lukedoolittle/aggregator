using System;
using System.Threading.Tasks;
using Material.Contracts;
using Material.Infrastructure;
using Material.Infrastructure.Credentials;

namespace Material.OAuth.Facade
{
    public class OAuth2CodeAuthorizationFacade : OAuth2AuthorizationFacadeBase
    {
        public OAuth2CodeAuthorizationFacade(
            OAuth2ResourceProvider resourceProvider, 
            string clientId, 
            Uri callbackUri, 
            IOAuth2AuthorizationAdapter oauth, 
            IOAuthSecurityStrategy strategy) : 
                base(
                    resourceProvider, 
                    clientId, 
                    callbackUri, 
                    oauth, 
                    strategy)
        { }

        protected override Task<OAuth2Credentials> GetRawAccessToken(
            OAuth2Credentials intermediateCredentials, 
            string secret)
        {
            if (intermediateCredentials == null) throw new ArgumentNullException(nameof(intermediateCredentials));

            ResourceProvider.SetClientProperties(
                ClientId,
                secret);

            return OAuth.GetAccessToken(
                ResourceProvider.TokenUrl,
                ClientId,
                secret,
                CallbackUri,
                intermediateCredentials.Code,
                ResourceProvider.Scope,
                ResourceProvider.Headers);
        }
    }
}
