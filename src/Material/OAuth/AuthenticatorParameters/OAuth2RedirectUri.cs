using System;
using Foundations.Extensions;
using Foundations.HttpClient.Authenticators;
using Foundations.HttpClient.Enums;

namespace Material.OAuth.AuthenticatorParameters
{
    public class OAuth2RedirectUri : IAuthenticatorParameter
    {
        public string Name => OAuth2Parameter.RedirectUri.EnumToString();
        public string Value { get; }

        public OAuth2RedirectUri(string redirectUri)
        {
            if (redirectUri == null) throw new ArgumentNullException(nameof(redirectUri));

            Value = redirectUri;
        }

        public OAuth2RedirectUri(Uri redirectUri) :
            this(redirectUri?.ToString())
        { }
    }
}
