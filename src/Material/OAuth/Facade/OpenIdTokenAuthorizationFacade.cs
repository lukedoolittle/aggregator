using System;
using System.Collections.Generic;
using Foundations.Extensions;
using Foundations.HttpClient.Enums;
using Material.Contracts;
using Material.Infrastructure;

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

        protected override IDictionary<string, string> GetSecurityParameters(string userId)
        {
            var baseSecurity = base.GetSecurityParameters(userId);

            var nonce = Strategy.CreateOrGetSecureParameter(
                userId,
                OAuth2Parameter.Nonce.EnumToString());

            baseSecurity.Add(
                OAuth2Parameter.Nonce.EnumToString(), 
                nonce);

            return baseSecurity;
        }
    }
}
