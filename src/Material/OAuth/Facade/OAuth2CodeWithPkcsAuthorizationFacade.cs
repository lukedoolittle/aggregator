using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Foundations.Extensions;
using Foundations.HttpClient.Authenticators;
using Foundations.HttpClient.Enums;
using Material.Contracts;
using Material.Infrastructure;
using Material.Infrastructure.Credentials;
using Material.OAuth.AuthenticatorParameters;

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

        protected override IList<IAuthenticatorParameter> GetSecurityParameters(
            string userId)
        {
            var securityParameters = base.GetSecurityParameters(userId);

            //TODO: support hashing besides plaintext
            var verifier = Strategy.CreateOrGetSecureParameter(
                userId,
                OAuth2Parameter.Verifier.EnumToString());

            securityParameters.Add(new OAuth2CodeChallenge(
                verifier));
            securityParameters.Add(new OAuth2CodeChallengeMethod(
                CodeChallengeMethod.Plain));

            return securityParameters;
        }

        protected override Task<OAuth2Credentials> GetRawAccessToken(
            OAuth2Credentials intermediateCredentials,
            string userId)
        {
            if (intermediateCredentials == null) throw new ArgumentNullException(nameof(intermediateCredentials));

            ResourceProvider.SetClientProperties(
                ClientId,
                null);

            var builder = new AuthenticatorBuilder()
                .AddParameter(new OAuth2ClientId(ClientId))
                .AddParameter(new OAuth2CodeVerifier(Strategy, userId))
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
