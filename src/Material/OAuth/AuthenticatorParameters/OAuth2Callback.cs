using System;
using Foundations.Extensions;
using Foundations.HttpClient.Authenticators;
using Foundations.HttpClient.Enums;

namespace Material.OAuth.AuthenticatorParameters
{
    public class OAuth2Callback : IAuthenticatorParameter
    {
        public string Name => OAuth2Parameter.RedirectUri.EnumToString();
        public string Value { get; }
        public HttpParameterType Type => HttpParameterType.Unspecified;

        public OAuth2Callback(Uri redirectUri)
        {
            if (redirectUri == null) throw new ArgumentNullException(nameof(redirectUri));

            Value = redirectUri.ToString();
        }
    }
}
