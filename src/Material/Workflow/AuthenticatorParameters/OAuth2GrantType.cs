using System;
using Material.Framework.Enums;
using Material.Framework.Extensions;
using Material.HttpClient.Authenticators;

namespace Material.Workflow.AuthenticatorParameters
{
    public class OAuth2GrantType : IAuthenticatorParameter
    {
        public string Name => OAuth2Parameter.GrantType.EnumToString();
        public string Value { get; }
        public HttpParameterType Type => HttpParameterType.Unspecified;

        public OAuth2GrantType(string grantType)
        {
            if (grantType == null) throw new ArgumentNullException(nameof(grantType));

            Value = grantType;
        }

        public OAuth2GrantType(GrantType grantType) : 
            this(grantType.EnumToString())
        { }
    }
}
