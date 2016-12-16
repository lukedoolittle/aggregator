using System;
using System.Collections.Generic;
using Foundations.HttpClient.Authenticators;
using Material.Contracts;
using Material.Infrastructure;
using Material.OAuth.AuthenticatorParameters;

namespace Material.OAuth.Facade
{
    public class OpenIdTokenAuthorizationFacade : OAuth2TokenAuthorizationFacade
    {
        public OpenIdTokenAuthorizationFacade(
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

        protected override IList<IAuthenticatorParameter> GetSecurityParameters(
            string userId)
        {
            var securityParameters = base.GetSecurityParameters(userId);

            securityParameters.Add(new OAuth2Nonce(Strategy, userId));

            return securityParameters;
        }
    }
}
