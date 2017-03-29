using System;
using Material.Framework.Enums;
using Material.Framework.Extensions;
using Material.HttpClient.Authenticators;

namespace Material.Workflow.AuthenticatorParameters
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
