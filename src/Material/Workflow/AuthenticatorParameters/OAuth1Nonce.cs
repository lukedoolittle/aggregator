using System;
using Material.Framework.Enums;
using Material.Framework.Extensions;
using Material.HttpClient.Authenticators;
using Material.HttpClient.Cryptography;
using Material.HttpClient.Cryptography.Enums;

namespace Material.Workflow.AuthenticatorParameters
{
    public class OAuth1Nonce : IAuthenticatorParameter
    {
        public string Name => OAuth1Parameter.Nonce.EnumToString();
        public string Value { get; }
        public HttpParameterType Type => HttpParameterType.Unspecified;

        public OAuth1Nonce(string nonce)
        {
            if (nonce == null) throw new ArgumentNullException(nameof(nonce));

            Value = nonce;
        }

        public OAuth1Nonce(ICryptoStringGenerator stringGenerator) : 
            this(stringGenerator?.CreateRandomString(
                        16,
                        CryptoStringType.LowercaseAlphanumeric))
        { }

        public OAuth1Nonce() : 
            this(new CryptoStringGenerator())
        { }
    }
}
