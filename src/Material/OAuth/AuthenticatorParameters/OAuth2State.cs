using System;
using Foundations.Extensions;
using Foundations.HttpClient.Authenticators;
using Foundations.HttpClient.Enums;

namespace Material.OAuth.AuthenticatorParameters
{
    public class OAuth2State : IAuthenticatorParameter
    {
        public string Name => OAuth2Parameter.State.EnumToString();
        public string Value { get; }
        public HttpParameterType Type => HttpParameterType.Unspecified;

        public OAuth2State(string state)
        {
            if (state == null) throw new ArgumentNullException(nameof(state));

            Value = state;
        }
    }
}
