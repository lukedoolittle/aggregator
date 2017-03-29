using System;
using Material.Framework.Enums;
using Material.Framework.Extensions;
using Material.HttpClient.Authenticators;
using Material.HttpClient.Cryptography.Algorithms;

namespace Material.Workflow.AuthenticatorParameters
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
