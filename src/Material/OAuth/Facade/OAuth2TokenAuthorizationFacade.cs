using System;
using System.Threading.Tasks;
using Material.Contracts;
using Material.Infrastructure;
using Material.Infrastructure.Credentials;

namespace Material.OAuth.Facade
{
    public class OAuth2TokenAuthorizationFacade : OAuth2AuthorizationFacadeBase
    {
        public OAuth2TokenAuthorizationFacade(
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
            string userId)
        {
            return Task.FromResult(intermediateCredentials);
        }
    }
}
