using System;
using Foundations.HttpClient.Cryptography.Keys;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Security;

namespace Foundations.HttpClient.Cryptography.Algorithms
{
    public class SigningAlgorithm : 
        ISigningAlgorithm, 
        IVerificationAlgorithm
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

        public byte[] SignText(
            byte[] text, 
            CryptoKey privateKey)
        {
            if (text == null) throw new ArgumentNullException(nameof(text));
            if (privateKey == null) throw new ArgumentNullException(nameof(privateKey));

            _signer.Reset();

            var privateKeyParameters = 
                PrivateKeyFactory.CreateKey(
                    privateKey.GetBytes());

            _signer.Init(true, privateKeyParameters);
            _signer.BlockUpdate(text, 0, text.Length);

            return _signer.GenerateSignature();
        }

        public bool VerifyText(
            CryptoKey key, 
            byte[] signature, 
            byte[] text)
        {
            if (key == null) throw new ArgumentNullException(nameof(key));
            if (signature == null) throw new ArgumentNullException(nameof(signature));
            if (text == null) throw new ArgumentNullException(nameof(text));

            _signer.Reset();

            var publicKeyParameters = 
                PublicKeyFactory.CreateKey(
                    key.GetBytes());

            _signer.Init(false, publicKeyParameters);
            _signer.BlockUpdate(text, 0, text.Length);

            return _signer.VerifySignature(signature);
        }
    }
}
