using System;
using Foundations.Extensions;
using Foundations.HttpClient.Authenticators;
using Foundations.HttpClient.Enums;

namespace Material.OAuth.AuthenticatorParameters
{
    public class OAuth2ClientSecret : IAuthenticatorParameter
    {
        public string Name => OAuth2Parameter.ClientSecret.EnumToString();
        public string Value { get; }
        public HttpParameterType Type => HttpParameterType.Unspecified;

        public OAuth2ClientSecret(string clientSecret)
        {
            if (clientSecret == null) throw new ArgumentNullException(nameof(clientSecret));

            Value = clientSecret;
        }
    }
}
