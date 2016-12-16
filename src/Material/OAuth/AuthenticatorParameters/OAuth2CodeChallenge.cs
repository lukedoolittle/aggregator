using System;
using Foundations.Extensions;
using Foundations.HttpClient.Authenticators;
using Foundations.HttpClient.Enums;

namespace Material.OAuth.AuthenticatorParameters
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
