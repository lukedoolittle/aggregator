using System;
using System.Collections.Generic;
using Material.Contracts;
using Material.Framework.Enums;
using Material.Framework.Extensions;
using Material.HttpClient.Authenticators;
using Material.Workflow.AuthenticatorParameters;

namespace Material.Workflow.Security
{
    public class OAuth2PlainPkceSecurityParameterBundle : ISecurityParameterBundle
    {
        public IEnumerable<IAuthenticatorParameter> GetBundle(
            IOAuthSecurityStrategy securityStrategy,
            string userId)
        {
            if (securityStrategy == null) throw new ArgumentNullException(nameof(securityStrategy));
            if (userId == null) throw new ArgumentNullException(nameof(userId));

            var verifier = securityStrategy.CreateSecureParameter(
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
