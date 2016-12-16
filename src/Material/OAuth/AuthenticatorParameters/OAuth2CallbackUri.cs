using System;
using Foundations.Extensions;
using Foundations.HttpClient.Authenticators;
using Foundations.HttpClient.Enums;

namespace Material.OAuth.AuthenticatorParameters
{
    public class OAuth2CallbackUri : IAuthenticatorParameter
    {
        public string Name => OAuth2Parameter.RedirectUri.EnumToString();
        public string Value { get; }
        public HttpParameterType Type => HttpParameterType.Unspecified;

        public OAuth2CallbackUri(string redirectUri)
        {
            if (redirectUri == null) throw new ArgumentNullException(nameof(redirectUri));

            Value = redirectUri;
        }

        public OAuth2CallbackUri(Uri redirectUri) :
            this(redirectUri?.ToString())
        { }
    }
}
