using System;
using System.Text;
using Foundations.HttpClient.Cryptography.Keys;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Digests;

namespace Foundations.HttpClient.Cryptography.Algorithms
{
    public class DigestSigningAlgorithm : ISigningAlgorithm
    {
        private readonly IDigest _digest;

        public string SignatureMethod { get; }


        public DigestSigningAlgorithm(IDigest digest) : 
            this(digest, null)
        { }

        public DigestSigningAlgorithm(
            IDigest digest,
            string signatureMethod)
        {
            if (digest == null) throw new ArgumentNullException(nameof(digest));

            _digest = digest;
            SignatureMethod = signatureMethod ?? digest.AlgorithmName.ToUpper();
        }

        public byte[] SignText(
            byte[] text, 
            CryptoKey privateKey)
        {
            if (text == null) throw new ArgumentNullException(nameof(text));

            _digest.Reset();

            if (privateKey != null)
            {
                var keyBytes = Encoding.UTF8.GetBytes(privateKey.ToString());
                _digest.BlockUpdate(keyBytes, 0, keyBytes.Length);
            }
            _digest.BlockUpdate(text, 0, text.Length);
            var result = new byte[_digest.GetDigestSize()];
            _digest.DoFinal(result, 0);

            return result;
        }

        public static DigestSigningAlgorithm Sha256Algorithm()
        {
            return new DigestSigningAlgorithm(
                new Sha256Digest(), 
                "SHA256");
        }
    }
}
