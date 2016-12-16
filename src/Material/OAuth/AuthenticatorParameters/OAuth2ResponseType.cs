using System;
using Foundations.Extensions;
using Foundations.HttpClient.Authenticators;
using Foundations.HttpClient.Enums;

namespace Material.OAuth.AuthenticatorParameters
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

        public OAuth2ResponseType(Foundations.HttpClient.Enums.OAuth2ResponseType responseType) : 
            this(responseType.EnumToString())
        { }
    }
}
