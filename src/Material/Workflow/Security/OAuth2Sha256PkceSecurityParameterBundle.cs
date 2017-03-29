using System;
using System.Collections.Generic;
using Material.Contracts;
using Material.Framework.Enums;
using Material.Framework.Extensions;
using Material.HttpClient.Authenticators;
using Material.HttpClient.Cryptography.Algorithms;
using Material.Workflow.AuthenticatorParameters;

namespace Material.Workflow.Security
{
    public class OAuth2Sha256PkceSecurityParameterBundle : ISecurityParameterBundle
    {
        private readonly ISigningAlgorithm _signingAlgorithm;

        public OAuth2Sha256PkceSecurityParameterBundle(
            ISigningAlgorithm signingAlgorithm)
        {
            _signingAlgorithm = signingAlgorithm;
        }

        public IEnumerable<IAuthenticatorParameter> GetBundle(
            IOAuthSecurityStrategy securityStrategy,
            string userId)
        {
            if (securityStrategy == null) throw new ArgumentNullException(nameof(securityStrategy));
            if (userId == null) throw new ArgumentNullException(nameof(userId));

            var verifier = securityStrategy.CreateSecureParameter(
                userId,
                OAuth2Parameter.Verifier.EnumToString());

            var codeChallenge = _signingAlgorithm
                .SignMessage(
                    verifier,
                    null)
                .ToBase64String()
                .Base64ToUrlEncodedBase64String();

            return new List<IAuthenticatorParameter>
            {
                new OAuth2CodeChallenge(codeChallenge),
                new OAuth2CodeChallengeMethod(CodeChallengeMethod.Sha256)
            };
        }
    }
}
