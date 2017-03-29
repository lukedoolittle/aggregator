using System;
using System.Text;
using Material.HttpClient.Cryptography.Keys;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Security;

namespace Material.HttpClient.Cryptography.Algorithms
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

        public byte[] SignMessage(
            string message, 
            CryptoKey privateKey)
        {
            if (message == null) throw new ArgumentNullException(nameof(message));
            if (privateKey == null) throw new ArgumentNullException(nameof(privateKey));

            _signer.Reset();

            var privateKeyParameters = 
                PrivateKeyFactory.CreateKey(
                    privateKey.GetBytes());

            var messageBytes = Encoding.UTF8.GetBytes(message);

            _signer.Init(true, privateKeyParameters);
            _signer.BlockUpdate(
                messageBytes, 
                0, 
                messageBytes.Length);

            return _signer.GenerateSignature();
        }

        public bool VerifyMessage(
            CryptoKey key, 
            byte[] signature, 
            string message)
        {
            if (key == null) throw new ArgumentNullException(nameof(key));
            if (signature == null) throw new ArgumentNullException(nameof(signature));
            if (message == null) throw new ArgumentNullException(nameof(message));

            _signer.Reset();

            var publicKeyParameters = 
                PublicKeyFactory.CreateKey(
                    key.GetBytes());

            _signer.Init(false, publicKeyParameters);

            var messageBytes = Encoding.UTF8.GetBytes(message);

            _signer.BlockUpdate(
                messageBytes, 
                0, 
                messageBytes.Length);

            return _signer.VerifySignature(signature);
        }
    }
}
