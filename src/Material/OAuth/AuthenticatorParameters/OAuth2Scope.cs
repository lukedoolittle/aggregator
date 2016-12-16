using System;
using System.Collections.Generic;
using Foundations.Extensions;
using Foundations.HttpClient.Authenticators;
using Foundations.HttpClient.Enums;

namespace Material.OAuth.AuthenticatorParameters
{
    public class OAuth2Scope : IAuthenticatorParameter
    {
        public string Name => OAuth2Parameter.Scope.EnumToString();
        public string Value { get; }
        public HttpParameterType Type => HttpParameterType.Unspecified;

        public OAuth2Scope(string scope)
        {
            if (scope == null) throw new ArgumentNullException(nameof(scope));

            Value = scope;
        }

        public OAuth2Scope(
            IEnumerable<string> scopes, 
            string separator) : 
            this(string.Join(separator, scopes))
        { }

        public OAuth2Scope(
            IEnumerable<string> scopes,
            char separator) :
            this(scopes, separator.ToString())
        { }
    }
}
