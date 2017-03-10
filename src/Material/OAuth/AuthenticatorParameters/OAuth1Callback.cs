using System;
using Foundations.Extensions;
using Foundations.HttpClient.Authenticators;
using Foundations.HttpClient.Enums;

namespace Material.OAuth.AuthenticatorParameters
{
    public class OAuth1Callback : IAuthenticatorParameter
    {
        public string Name => OAuth1Parameter.Callback.EnumToString();
        public string Value { get; }
        public HttpParameterType Type => HttpParameterType.Unspecified;

        public OAuth1Callback(Uri redirectUri)
        {
            if (redirectUri == null) throw new ArgumentNullException(nameof(redirectUri));

            Value = redirectUri.ToCorrectedString();
        }

        public OAuth1Callback(Uri redirectUri, string userId) :
            this(redirectUri.AddPath(userId))
        { }
    }
}
