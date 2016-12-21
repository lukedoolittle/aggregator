using System;
using Foundations.Extensions;
using Foundations.HttpClient.Cryptography.Discovery;
using Foundations.HttpClient.Cryptography.Enums;
using Foundations.HttpClient.Cryptography.Keys;
using Org.BouncyCastle.Utilities.Encoders;

namespace Foundations.HttpClient.Extensions
{
    public static class CryptoKeyExtensions
    {
        public static CryptoKey ToCryptoKey(
            this JsonWebKey instance)
        {
            if (instance == null) throw new ArgumentNullException(nameof(instance));

            if (instance.KeyType == EncryptionAlgorithm.EllipticCurve.EnumToString())
            {
                return new EcdsaCryptoKey(
                    "ECDSA",
                    instance.CurveName,
                    Base64.Decode(instance.X.UrlEncodedBase64ToBase64String()),
                    Base64.Decode(instance.Y.UrlEncodedBase64ToBase64String()));
            }
            else if (instance.KeyType == EncryptionAlgorithm.RSA.EnumToString())
            {
                return new RsaCryptoKey(
                    Base64.Decode(instance.N.UrlEncodedBase64ToBase64String()),
                    Base64.Decode(instance.E.UrlEncodedBase64ToBase64String()));
            }

            return null;
        }
    }
}
