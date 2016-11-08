using System;
using System.Globalization;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Math;

namespace Foundations.HttpClient.Cryptography
{
    public class RSASignatureVerificationAlgorithm : ISignatureVerificationAlgorithm
    {
        private readonly ISigner _signer;

        public RSASignatureVerificationAlgorithm(ISigner signer)
        {
            if (signer == null)
            {
                throw new ArgumentNullException(nameof(signer));
            }
            _signer = signer;
        }

        public bool VerifyText(
            string modulus, 
            string publicExponent, 
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

            var parameters = new RsaKeyParameters(
                false,
                new BigInteger(modulus),
                new BigInteger(publicExponent));

            _signer.Init(false, parameters);
            _signer.BlockUpdate(text, 0, text.Length);

            return _signer.VerifySignature(signature);
        }
    }
}
