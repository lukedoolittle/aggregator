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

        public OAuth2Scope(string scope)
        {
            Value = scope;
        }

        public OAuth2Scope(
            IEnumerable<string> scopes, 
            string seperator) : 
            this(string.Join(seperator, scopes))
        { }
    }
}
