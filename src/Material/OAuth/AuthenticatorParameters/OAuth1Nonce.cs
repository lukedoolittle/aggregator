using Foundations.Extensions;
using Foundations.HttpClient.Authenticators;
using Foundations.HttpClient.Cryptography;
using Foundations.HttpClient.Cryptography.Enums;
using Foundations.HttpClient.Enums;

namespace Material.OAuth.AuthenticatorParameters
{
    public class OAuth1Nonce : IAuthenticatorParameter
    {
        public string Name => OAuth1Parameter.Nonce.EnumToString();
        public string Value { get; }

        public OAuth1Nonce(string nonce)
        {
            Value = nonce;
        }

        public OAuth1Nonce(ICryptoStringGenerator stringGenerator) : 
            this(stringGenerator?.CreateRandomString(
                        16,
                        CryptoStringType.LowercaseAlphanumeric))
        { }
    }
}
