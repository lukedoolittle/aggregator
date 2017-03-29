using System;
using Material.Framework.Enums;
using Material.Framework.Extensions;
using Material.HttpClient.Authenticators;

namespace Material.Workflow.AuthenticatorParameters
{
    public class OAuth2ResponseType : IAuthenticatorParameter
    {
        public string Name => OAuth2Parameter.ResponseType.EnumToString();
        public string Value { get; }
        public HttpParameterType Type => HttpParameterType.Unspecified;

        public OAuth2ResponseType(string responseType)
        {
            if (responseType == null) throw new ArgumentNullException(nameof(responseType));

            Value = responseType;
        }

        public OAuth2ResponseType(Framework.Enums.OAuth2ResponseType responseType) : 
            this(responseType.EnumToString())
        { }
    }
}
