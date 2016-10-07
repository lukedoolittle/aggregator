using System;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Digests;
using Org.BouncyCastle.Crypto.Prng;
using Org.BouncyCastle.Security;

namespace Foundations.Cryptography.StringCreation
{
    public class CryptoStringGenerator : ICryptoStringGenerator
    {
        public string CreateRandomString(
            int stringLength, 
            CryptoStringType stringType)
        {
            //Per NIST truncating a SHA256 hash by taking the left most n bits is acceptable
            //http://nvlpubs.nist.gov/nistpubs/Legacy/SP/nistspecialpublication800-107r1.pdf
            return CreateCryptographicallyStrongString<Sha256Digest>(
                stringType)
                .Substring(0, stringLength);
        }

        /// <summary>
        /// Creates a cryptographically strong string with the given digest
        /// </summary>
        /// <typeparam name="TDigest">Hash type</typeparam>
        /// <param name="stringType">Filter to apply to string</param>
        /// <returns>A string with only the characters defined by stringType</returns>
        private static string CreateCryptographicallyStrongString<TDigest>(
            CryptoStringType stringType)
            where TDigest : IDigest, new()
        {
            var randomBytes = BitConverter.GetBytes(
                new SecureRandom().NextInt());

            var digest = new TDigest();
            digest.BlockUpdate(
                randomBytes,
                0,
                randomBytes.Length);
            var randomDigest = new DigestRandomGenerator(digest);

            var cryptoData = new byte[digest.GetDigestSize()];
            randomDigest.NextBytes(cryptoData);

            var result = Convert.ToBase64String(cryptoData);


            switch (stringType)
            {
                case CryptoStringType.Base64Alphanumeric:
                    return result
                        .Replace('/', '_')
                        .Replace('+', '-');
                //Note that this substitution reduces the integrity of the strong string
                //but is necessary in certain URL scenarios
                case CryptoStringType.LowercaseAlphanumeric:
                    return result
                        .Replace('/', 'a')
                        .Replace('+', 'b')
                        .ToLower();
                case CryptoStringType.Base64:
                default:
                    return result;
            }
        }
    }
}
