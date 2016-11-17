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

        public RsaCryptoKey(byte[] modulus, byte[] publicExponent) :
            base(new RsaKeyParameters(
                false,
                new BigInteger(1, modulus),
                new BigInteger(1, publicExponent)))
        { }

        public RsaCryptoKey(string modulus, string publicExponent) :
            base(new RsaKeyParameters(
                false,
                new BigInteger(modulus),
                new BigInteger(publicExponent)))
        { }

        public RsaCryptoKey(
            BigInteger modulus, 
            BigInteger publicExponent) :
                base(new RsaKeyParameters(
                    false,
                    modulus,
                    publicExponent))
        {
            if (modulus == null) throw new ArgumentNullException(nameof(modulus));
            if (publicExponent == null) throw new ArgumentNullException(nameof(publicExponent));

            Modulus = modulus.ToString();
            Exponent = publicExponent.ToString();
        }

        public RsaCryptoKey(string key, bool isPrivate) : 
            base(key, isPrivate)
        { }
    }
}
