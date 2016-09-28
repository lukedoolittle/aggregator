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
            _signer.Init(true, GetParametersFromKey(privateKey));

            _signer.BlockUpdate(text, 0, text.Length);
            var signature = _signer.GenerateSignature();

            return signature;
        }

        private const string PrivateKeyPrefix = "-----BEGIN PRIVATE KEY-----";
        private const string PrivateKeySuffix = "-----END PRIVATE KEY-----";

        private static ICipherParameters GetParametersFromKey(string privateKey)
        {
            var base64PrivateKey = privateKey.Replace(PrivateKeyPrefix, "").Replace("\n", "").Replace(PrivateKeySuffix, "");
            var privateKeyBytes = Convert.FromBase64String(base64PrivateKey);
            return PrivateKeyFactory.CreateKey(privateKeyBytes);
        }
    }
}
