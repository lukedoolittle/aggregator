using System;
using Foundations.Extensions;
using Foundations.HttpClient.Authenticators;
using Foundations.HttpClient.Enums;

namespace Material.OAuth.AuthenticatorParameters
{
    public class OAuth2Code : IAuthenticatorParameter
    {
        public string Name => 
            Foundations.HttpClient.Enums.OAuth2ResponseType.Code.EnumToString();
        public string Value { get; }
        public HttpParameterType Type => HttpParameterType.Unspecified;

        public OAuth2Code(string code)
        {
            if (code == null) throw new ArgumentNullException(nameof(code));

            Value = code;
        }
    }
}
