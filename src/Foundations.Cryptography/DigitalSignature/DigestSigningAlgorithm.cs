using System;
using System.Text;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Digests;
using Org.BouncyCastle.Crypto.Macs;
using Org.BouncyCastle.Crypto.Parameters;

namespace Foundations.Cryptography.DigitalSignature
{
    public class DigestSigningAlgorithm : ISigningAlgorithm
    {
        private readonly IDigest _digest;

        public string SignatureMethod { get; }

        public DigestSigningAlgorithm(
            IDigest digest,
            string signatureMethod = null)
        {
            if (digest == null)
            {
                throw new ArgumentNullException(nameof(digest));
            }
            _digest = digest;
            SignatureMethod = signatureMethod ?? "HMAC" + digest.AlgorithmName.ToUpper();
        }

        public byte[] SignText(byte[] text, string privateKey)
        {
            var hmac = new HMac(_digest);
            hmac.Init(new KeyParameter(Encoding.UTF8.GetBytes(privateKey)));
            var result = new byte[hmac.GetMacSize()];
            
            hmac.BlockUpdate(text, 0, text.Length);
            hmac.DoFinal(result, 0);

            return result;
        }

        public bool VerifyText(
            string publicKey, 
            byte[] signature, 
            byte[] text)
        {
            throw new NotImplementedException("Cannot verify Hash");
        }

        public static ISigningAlgorithm Sha1Algorithm()
        {
            return new DigestSigningAlgorithm(new Sha1Digest(), "HMAC-SHA1");
        }
    }
}
