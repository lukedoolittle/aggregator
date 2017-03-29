using System;
using Material.Framework.Enums;
using Material.Framework.Extensions;
using Material.HttpClient.Authenticators;

namespace Material.Workflow.AuthenticatorParameters
{
    public class OAuth2RefreshToken : IAuthenticatorParameter
    {
        public string Name => OAuth2Parameter.RefreshToken.EnumToString();
        public string Value { get; }
        public HttpParameterType Type => HttpParameterType.Unspecified;

        public OAuth2RefreshToken(string refreshToken)
        {
            if (refreshToken == null) throw new ArgumentNullException(nameof(refreshToken));

            Value = refreshToken;
        }
    }
}
