using System;
using Material.Framework.Enums;
using Material.Framework.Extensions;
using Material.HttpClient.Authenticators;

namespace Material.Workflow.AuthenticatorParameters
{
    public class OAuth2Callback : IAuthenticatorParameter
    {
        public string Name => OAuth2Parameter.RedirectUri.EnumToString();
        public string Value { get; }
        public HttpParameterType Type => HttpParameterType.Unspecified;

        public OAuth2Callback(Uri redirectUri)
        {
            if (redirectUri == null) throw new ArgumentNullException(nameof(redirectUri));

            Value = redirectUri.ToCorrectedString();
        }
    }
}
