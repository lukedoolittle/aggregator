using System;
using System.Collections.Generic;
using Foundations.Extensions;
using Foundations.HttpClient.Authenticators;
using Foundations.HttpClient.Enums;
using Material.Contracts;
using Material.OAuth.AuthenticatorParameters;

namespace Material.OAuth.Security
{
    public class OAuth2PlainPkceSecurityParameterBundle
    {
        public IEnumerable<IAuthenticatorParameter> GetBundle(
            IOAuthSecurityStrategy securityStrategy,
            string userId)
        {
            if (securityStrategy == null) throw new ArgumentNullException(nameof(securityStrategy));
            if (userId == null) throw new ArgumentNullException(nameof(userId));

            var verifier = securityStrategy.CreateOrGetSecureParameter(
                userId,
                OAuth2Parameter.Verifier.EnumToString());

            return new List<IAuthenticatorParameter>
            {
                new OAuth2CodeChallenge(verifier),
                new OAuth2CodeChallengeMethod(CodeChallengeMethod.Plain)
            };
        }
    }
}
