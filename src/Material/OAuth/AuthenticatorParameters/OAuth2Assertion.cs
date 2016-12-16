using System;
using Foundations.Extensions;
using Foundations.HttpClient.Authenticators;
using Foundations.HttpClient.Enums;

namespace Material.OAuth.AuthenticatorParameters
{
    public class OAuth2Assertion : IAuthenticatorParameter
    {
        public string Name => OAuth2Parameter.Assertion.EnumToString();
        public string Value { get; }

        //TODO: put the web token in here and perform the signing

        public OAuth2Assertion(string assertion)
        {
            if (assertion == null) throw new ArgumentNullException(nameof(assertion));

            Value = assertion;
        }
    }
}
