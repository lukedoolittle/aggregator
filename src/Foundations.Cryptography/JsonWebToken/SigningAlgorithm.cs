using System;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Security;

namespace Foundations.Cryptography.JsonWebToken
{
    public class SigningAlgorithm : ISigningAlgorithm
    {
        private readonly ISigner _signer;

        public SigningAlgorithm(ISigner signer)
        {
            _signer = signer;
        }

        public byte[] SignText(byte[] text, string privateKey)
        {
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
