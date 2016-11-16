using System;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Math;

namespace Foundations.HttpClient.Cryptography.Keys
{
    public class RsaCryptoKey : CryptoKey
    {
        public string Modulus { get; }
        public string Exponent { get; }

        public RsaCryptoKey(RsaKeyParameters key) :
            base(key)
        {
            if (key == null) throw new ArgumentNullException(nameof(key));

            Modulus = key.Modulus.ToString();
            Exponent = key.Exponent.ToString();
        }

        public RsaCryptoKey(string modulus, string publicExponent) :
            base(new RsaKeyParameters(
                false,
                new BigInteger(modulus),
                new BigInteger(publicExponent)))
        {
            Modulus = modulus;
            Exponent = publicExponent;
        }

        public RsaCryptoKey(string key, bool isPrivate) : 
            base(key, isPrivate)
        { }
    }
}
