using System;
using Foundations.Extensions;
using Foundations.HttpClient.Authenticators;
using Foundations.HttpClient.Enums;
using Material.Contracts;

namespace Material.OAuth.AuthenticatorParameters
{
    public class OAuth1Verifier : IAuthenticatorParameter
    {
        public string Name => OAuth1Parameter.Verifier.EnumToString();
        public string Value { get; }
        public HttpParameterType Type => HttpParameterType.Unspecified;

        public OAuth1Verifier(string verifier)
        {
            Value = verifier;
        }
    }
}
