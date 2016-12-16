using System;
using Foundations.Extensions;
using Foundations.HttpClient.Authenticators;
using Foundations.HttpClient.Enums;

namespace Material.OAuth.AuthenticatorParameters
{
    public class OAuth1CallbackUri : IAuthenticatorParameter
    {
        public string Name => OAuth1Parameter.Callback.EnumToString();
        public string Value { get; }

        public OAuth1CallbackUri(string redirectUri)
        {
            if (redirectUri == null) throw new ArgumentNullException(nameof(redirectUri));

            Value = redirectUri;
        }

        public OAuth1CallbackUri(Uri redirectUri) : 
            this(redirectUri?.ToString())
        { }
    }
}
