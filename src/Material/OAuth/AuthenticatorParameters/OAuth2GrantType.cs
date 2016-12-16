using System;
using Foundations.Extensions;
using Foundations.HttpClient.Authenticators;
using Foundations.HttpClient.Enums;

namespace Material.OAuth.AuthenticatorParameters
{
    public class OAuth2GrantType : IAuthenticatorParameter
    {
        public string Name => OAuth2Parameter.GrantType.EnumToString();
        public string Value { get; }

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
