using System;
using Foundations.Extensions;
using Foundations.HttpClient.Authenticators;
using Foundations.HttpClient.Cryptography.Algorithms;
using Foundations.HttpClient.Enums;

namespace Material.OAuth.AuthenticatorParameters
{
    public class OAuth1SignatureMethod : IAuthenticatorParameter
    {
        public string Name => OAuth1Parameter.SignatureMethod.EnumToString();
        public string Value { get; }
        public HttpParameterType Type => HttpParameterType.Unspecified;

        public OAuth1SignatureMethod(string method)
        {
            if (method == null) throw new ArgumentNullException(nameof(method));

            Value = method;
        }

        public OAuth1SignatureMethod(ISigningAlgorithm signingAlgorithm) : 
            this(signingAlgorithm?.SignatureMethod)
        { }
    }
}
