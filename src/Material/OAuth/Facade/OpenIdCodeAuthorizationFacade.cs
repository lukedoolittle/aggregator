using System;
using System.Collections.Generic;
using Foundations.Extensions;
using Foundations.HttpClient.Enums;
using Material.Contracts;
using Material.Infrastructure;

namespace Material.OAuth.Facade
{
    public class OpenIdCodeAuthorizationFacade : OAuth2CodeAuthorizationFacade
    {
        public OpenIdCodeAuthorizationFacade(
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
