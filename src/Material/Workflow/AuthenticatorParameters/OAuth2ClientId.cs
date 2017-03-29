using System;
using Material.Framework.Enums;
using Material.Framework.Extensions;
using Material.HttpClient.Authenticators;

namespace Material.Workflow.AuthenticatorParameters
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
