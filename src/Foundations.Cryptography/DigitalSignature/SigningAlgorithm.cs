using System;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Security;

namespace Foundations.Cryptography.DigitalSignature
{
    public class SigningAlgorithm : ISigningAlgorithm
    {
        private readonly ISigner _signer;

        public SigningAlgorithm(ISigner signer)
        {
            if (signer == null)
            {
                throw new ArgumentNullException(nameof(signer));
            }
            _signer = signer;
        }

        public string SignatureMethod => _signer.AlgorithmName.ToUpper();

        public byte[] SignText(byte[] text, string privateKey)
        {
            if (text == null)
            {
                throw new ArgumentNullException(nameof(text));
            }

            _signer.Reset();

            _signer.Init(true, GetParametersFromPrivateKey(privateKey));
            _signer.BlockUpdate(text, 0, text.Length);

            return _signer.GenerateSignature();
        }

        public bool VerifyText(
            string publicKey, 
            byte[] signature, 
            byte[] text)
        {
            if (text == null)
            {
                throw new ArgumentNullException(nameof(text));
            }
            if (signature == null)
            {
                throw new ArgumentNullException(nameof(text));
            }

            _signer.Reset();

            _signer.Init(false, GetParametersFromPublicKey(publicKey));
            _signer.BlockUpdate(text, 0, text.Length);

            return _signer.VerifySignature(signature);
        }

        private const string PrivateKeyPrefix = "-----BEGIN PRIVATE KEY-----";
        private const string PrivateKeySuffix = "-----END PRIVATE KEY-----";

        private static ICipherParameters GetParametersFromPrivateKey(string privateKey)
        {
            var base64PrivateKey = privateKey.Replace(PrivateKeyPrefix, "").Replace("\n", "").Replace(PrivateKeySuffix, "");
            var privateKeyBytes = Convert.FromBase64String(base64PrivateKey);
            return PrivateKeyFactory.CreateKey(privateKeyBytes);
        }

        private const string PublicKeyPrefix = "-----BEGIN PUBLIC KEY-----";
        private const string PublicKeySuffix = "-----END PUBLIC KEY-----";

        private static ICipherParameters GetParametersFromPublicKey(string publicKey)
        {
            var base64PrivateKey = publicKey.Replace(PublicKeyPrefix, "").Replace("\n", "").Replace(PublicKeySuffix, "");
            var privateKeyBytes = Convert.FromBase64String(base64PrivateKey);
            return PublicKeyFactory.CreateKey(privateKeyBytes);
        }
    }
}
