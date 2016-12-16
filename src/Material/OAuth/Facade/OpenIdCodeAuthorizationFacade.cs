using System;
using System.Collections.Generic;
using Foundations.HttpClient.Authenticators;
using Material.Contracts;
using Material.Infrastructure;
using Material.OAuth.AuthenticatorParameters;

namespace Material.OAuth.Facade
{
    public class OpenIdCodeAuthorizationFacade : OAuth2CodeAuthorizationFacade
    {
        public OpenIdCodeAuthorizationFacade(
            OAuth2ResourceProvider resourceProvider, 
            string clientId, 
            string clientSecret,
            Uri callbackUri, 
            IOAuth2AuthorizationAdapter oauth, 
            IOAuthSecurityStrategy strategy) : 
                base(
                    resourceProvider, 
                    clientId, 
                    clientSecret,
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
