using System;
using System.Linq;
using System.Text;
using Material.HttpClient.Cryptography.Keys;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Digests;
using Org.BouncyCastle.Crypto.Macs;
using Org.BouncyCastle.Crypto.Parameters;

namespace Material.HttpClient.Cryptography.Algorithms
{
    public class HmacDigestSigningAlgorithm : 
        ISigningAlgorithm, 
        IVerificationAlgorithm
    {
        private readonly IDigest _digest;

        public string SignatureMethod { get; }

        public HmacDigestSigningAlgorithm(IDigest digest) : 
            this(digest, null)
        { }

        public HmacDigestSigningAlgorithm(
            IDigest digest,
            string signatureMethod)
        {
            if (digest == null) throw new ArgumentNullException(nameof(digest));

            _digest = digest;
            SignatureMethod = signatureMethod ?? "HMAC" + digest.AlgorithmName.ToUpper();
        }

        public byte[] SignMessage(
            string message, 
            CryptoKey privateKey)
        {
            if (message == null) throw new ArgumentNullException(nameof(message));
            if (privateKey == null) throw new ArgumentNullException(nameof(privateKey));

            var hmac = new HMac(_digest);
            hmac.Init(new KeyParameter(privateKey.GetBytes()));
            var result = new byte[hmac.GetMacSize()];

            var messageBytes = Encoding.UTF8.GetBytes(message);

            hmac.BlockUpdate(
                messageBytes, 
                0, 
                messageBytes.Length);
            hmac.DoFinal(result, 0);

            return result;
        }

        public bool VerifyMessage(
            CryptoKey key,
            byte[] signature,
            string message)
        {
            return signature.SequenceEqual(
                SignMessage(
                    message, 
                    key));
        }

        public static ISigningAlgorithm Sha1Algorithm()
        {
            return new HmacDigestSigningAlgorithm(new Sha1Digest(), "HMAC-SHA1");
        }

        public static ISigningAlgorithm Sha256Algorithm()
        {
            return new HmacDigestSigningAlgorithm(new Sha256Digest());
        }
    }
}
