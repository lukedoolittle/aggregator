using System;
using Foundations.Extensions;
using Foundations.HttpClient.Authenticators;
using Foundations.HttpClient.Enums;

namespace Material.OAuth.AuthenticatorParameters
{
    public class OAuth2ClientId : IAuthenticatorParameter
    {
        public string Name => OAuth2Parameter.ClientId.EnumToString();
        public string Value { get; }
        public HttpParameterType Type => HttpParameterType.Unspecified;

        public OAuth2ClientId(string clientId)
        {
            if (clientId == null) throw new ArgumentNullException(nameof(clientId));

            Value = clientId;
        }
    }
}
