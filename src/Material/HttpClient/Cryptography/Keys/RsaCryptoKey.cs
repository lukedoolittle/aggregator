using System;
using Material.Framework.Extensions;
using Material.HttpClient.Cryptography.Enums;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Utilities.Encoders;

namespace Material.HttpClient.Cryptography.Keys
{
    public class RsaCryptoKey : CryptoKey
    {
        public string Modulus { get; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1819:PropertiesShouldNotReturnArrays")]
        public byte[] ModulusBytes { get; }
        public string Exponent { get; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1819:PropertiesShouldNotReturnArrays")]
        public byte[] ExponentBytes { get; }

        public RsaCryptoKey(RsaKeyParameters key) :
            base(
                key, 
                StringEncoding.Base64Url)
        {
            if (key == null) throw new ArgumentNullException(nameof(key));

            Modulus = key.Modulus.ToString();
            ModulusBytes = key.Modulus.ToByteArray();
            Exponent = key.Exponent.ToString();
            ExponentBytes = key.Exponent.ToByteArray();
        }

        public RsaCryptoKey(
            string modulus, 
            string publicExponent) :
                this(new RsaKeyParameters(
                    false,
                    new BigInteger(
                        1,
                        Base64.Decode(
                            modulus.UrlEncodedBase64ToBase64String())),
                    new BigInteger(
                        1,
                        Base64.Decode(
                            publicExponent.UrlEncodedBase64ToBase64String()))))
        { }

        public RsaCryptoKey(
            string key,
            bool isPrivate, 
            StringEncoding encoding) : 
                base(
                    key, 
                    isPrivate,
                    encoding)
        { }
    }
}
