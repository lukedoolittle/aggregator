using System;
using Material.Framework.Enums;
using Material.Framework.Extensions;
using Material.HttpClient.Authenticators;

namespace Material.Workflow.AuthenticatorParameters
{
    public class OAuth2Code : IAuthenticatorParameter
    {
        public string Name => 
            Framework.Enums.OAuth2ResponseType.Code.EnumToString();
        public string Value { get; }
        public HttpParameterType Type => HttpParameterType.Unspecified;

        public OAuth2Code(string code)
        {
            if (code == null) throw new ArgumentNullException(nameof(code));

            Value = code;
        }
    }
}
