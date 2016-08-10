using System;
using System.Text;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Digests;
using Org.BouncyCastle.Crypto.Macs;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Crypto.Prng;
using Org.BouncyCastle.Security;

namespace Foundations.Cryptography
{
    public static class Security
    {
        public static Guid CreateGuidFromData(string data)
        {
            var digest = new MD5Digest();

            var bytes = Encoding.UTF8.GetBytes(data);
            digest.BlockUpdate(bytes, 0, bytes.Length);

            var result = new byte[digest.GetDigestSize()];
            digest.DoFinal(result, 0);

            return new Guid(result);
        }

        /// <summary>
        /// Creates a cryptographically strong string
        /// </summary>
        /// <returns></returns>
        public static string Create16CharacterCryptographicallyStrongString(
            CryptoStringTypeEnum stringType = CryptoStringTypeEnum.LowercaseAlphaNumeric)
        {
            return CreateCryptographicallyStrongString<Sha256Digest>(
                stringType)
                .Substring(0, 16);
        }

        public static string Create32CharacterCryptographicallyStrongString(
            CryptoStringTypeEnum stringType = CryptoStringTypeEnum.Base64AlphaNumeric)
        {
            return CreateCryptographicallyStrongString<Sha256Digest>(
                stringType);
        }

        /// <summary>
        /// Creates a cryptographically strong string with the given digest
        /// </summary>
        /// <typeparam name="TDigest">Hash type</typeparam>
        /// <param name="stringType">Filter to apply to string</param>
        /// <returns>A string with only the characters defined by stringType</returns>
        private static string CreateCryptographicallyStrongString<TDigest>(
            CryptoStringTypeEnum stringType)
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
                case CryptoStringTypeEnum.Base64AlphaNumeric:
                    return result
                        .Replace('/', '_')
                        .Replace('+', '-');
                case CryptoStringTypeEnum.LowercaseAlphaNumeric:
                    return result
                        .Replace('/', 'a')
                        .Replace('+', 'b')
                        .ToLower();
                case CryptoStringTypeEnum.Base64:
                default:
                    return result;
            }
        }

        /// <summary>
        /// Performs a SHA1 Hash
        /// </summary>
        /// <param name="key">Key</param>
        /// <param name="value">SHA value</param>
        /// <returns>Base 64 encoded string representing the hash</returns>
        public static string Sha1Hash(
            string key, 
            string value)
        {
            var hmac = new HMac(new Sha1Digest());
            hmac.Init(new KeyParameter(Encoding.UTF8.GetBytes(key)));
            var result = new byte[hmac.GetMacSize()];
            var bytes = Encoding.UTF8.GetBytes(value);

            hmac.BlockUpdate(bytes, 0, bytes.Length);
            hmac.DoFinal(result, 0);

            return Convert.ToBase64String(result);
        }
    }
}
