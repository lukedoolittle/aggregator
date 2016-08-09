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
            MD5Digest digest = new MD5Digest();

            byte[] msgBytes = Encoding.UTF8.GetBytes(data);
            digest.BlockUpdate(msgBytes, 0, msgBytes.Length);
            byte[] result = new byte[digest.GetDigestSize()];
            digest.DoFinal(result, 0);
            return new Guid(result);
        }

        public static string CreateCryptographicallyStrongString<TDigest>(
            int stringLength = 32)
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

            return Convert.ToBase64String(cryptoData)
                .Substring(
                    0, 
                    stringLength);
        }

        public static int RandomNumberBetween(
            int minValue,
            int maxValue)
        {
            return new SecureRandom().Next(minValue, maxValue);
        }

        public static string Sha1Hash(string key, string value)
        {
            var hmac = new HMac(new Sha1Digest());
            hmac.Init(new KeyParameter(Encoding.UTF8.GetBytes(key)));
            byte[] result = new byte[hmac.GetMacSize()];
            byte[] bytes = Encoding.UTF8.GetBytes(value);

            hmac.BlockUpdate(bytes, 0, bytes.Length);
            hmac.DoFinal(result, 0);

            return Convert.ToBase64String(result);
        }

        private const string DIGIT = "1234567890";
        private const string LOWER = "abcdefghijklmnopqrstuvwxyz";
        private static readonly string _chars = LOWER + DIGIT;

        public static string GetNonce(int nonceSize)
        {
            var nonce = new char[nonceSize];

            for (var i = 0; i < nonce.Length; i++)
            {
                var randomIndex = Security.RandomNumberBetween(
                    0,
                    _chars.Length);
                nonce[i] = _chars[randomIndex];
            }

            return new string(nonce);
        }
    }
}
