using System;
using Foundations.Extensions;
using Foundations.HttpClient.Cryptography.Discovery;
using Foundations.HttpClient.Cryptography.Enums;
using Foundations.HttpClient.Cryptography.Keys;

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
                    instance.X,
                    instance.Y);
            }
            else if (instance.KeyType == EncryptionAlgorithm.RSA.EnumToString())
            {
                return new RsaCryptoKey(
                    instance.N,
                    instance.E);
            }

            return null;
        }
    }
}
