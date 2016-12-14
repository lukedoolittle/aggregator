using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Foundations.Extensions;
using Foundations.HttpClient.Enums;
using Material.Contracts;
using Material.Infrastructure;
using Material.Infrastructure.Credentials;

namespace Material.OAuth.Facade
{
    class OAuth2CodeWithPkcsAuthorizationFacade : OAuth2AuthorizationFacadeBase
    {
        public OAuth2CodeWithPkcsAuthorizationFacade(
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

        protected override IDictionary<string, string> GetSecurityParameters(
            string userId)
        {
            var securityParameters = base.GetSecurityParameters(userId);

            var verifier = Strategy.CreateOrGetSecureParameter(
                userId,
                OAuth2Parameter.Verifier.EnumToString());

            //TODO: support hashing besides plaintext

            securityParameters.Add(
                OAuth2Parameter.Challenge.EnumToString(), 
                verifier);
            securityParameters.Add(
                OAuth2Parameter.ChallengeMethod.EnumToString(), 
                CodeChallengeMethod.Plain.EnumToString());

            return securityParameters;
        }

        protected override Task<OAuth2Credentials> GetRawAccessToken(
            OAuth2Credentials intermediateCredentials,
            string userId)
        {
            if (intermediateCredentials == null) throw new ArgumentNullException(nameof(intermediateCredentials));

            var verifier = Strategy.CreateOrGetSecureParameter(
                userId,
                OAuth2Parameter.Verifier.EnumToString());

            ResourceProvider.SetClientProperties(
                ClientId,
                null);

            return OAuth.GetAccessToken(
                ResourceProvider.TokenUrl,
                ClientId,
                null,
                verifier,
                CallbackUri,
                intermediateCredentials.Code,
                ResourceProvider.Scope,
                ResourceProvider.Headers);
        }
    }
}
