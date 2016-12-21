using System;
using System.Collections.Generic;
using System.Text;
using Foundations.Extensions;
using Foundations.HttpClient.Authenticators;
using Foundations.HttpClient.Cryptography.Algorithms;
using Foundations.HttpClient.Enums;
using Material.Contracts;
using Material.OAuth.AuthenticatorParameters;

namespace Material.OAuth.Security
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

            var verifier = securityStrategy.CreateOrGetSecureParameter(
                userId,
                OAuth2Parameter.Verifier.EnumToString());

            var codeChallenge = _signingAlgorithm
                .SignText(
                    Encoding.UTF8.GetBytes(verifier),
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
