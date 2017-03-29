using System;
using Material.Framework.Enums;
using Material.Framework.Extensions;
using Material.HttpClient.Authenticators;

namespace Material.Workflow.AuthenticatorParameters
{
    public class OAuth2CodeChallenge : IAuthenticatorParameter
    {
        public string Name => OAuth2Parameter.Challenge.EnumToString();
        public string Value { get; }
        public HttpParameterType Type => HttpParameterType.Unspecified;

        public OAuth2CodeChallenge(string challenge)
        {
            if (challenge == null) throw new ArgumentNullException(nameof(challenge));

            Value = challenge;
        }
    }
}
