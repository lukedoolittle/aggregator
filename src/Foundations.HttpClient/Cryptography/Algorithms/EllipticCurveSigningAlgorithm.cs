using System;
using System.IO;
using Foundations.Extensions;
using Foundations.HttpClient.Cryptography.Keys;
using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Math;

namespace Foundations.HttpClient.Cryptography.Algorithms
{
    public class EllipticCurveSigningAlgorithm : 
        ISigningAlgorithm, 
        IVerificationAlgorithm
    {
        public string SignatureMethod => _signer.AlgorithmName.ToUpper();

        private readonly ISigner _signer;

        public EllipticCurveSigningAlgorithm(ISigner signer)
        {
            if (signer == null)
            {
                throw new ArgumentNullException(nameof(signer));
            }
            _signer = signer;
        }

        public byte[] SignText(
            byte[] text,
            CryptoKey privateKey)
        {
            if (text == null) throw new ArgumentNullException(nameof(text));
            if (privateKey == null) throw new ArgumentNullException(nameof(privateKey));

            _signer.Reset();

            _signer.Init(true, privateKey.GetParameter<ECPrivateKeyParameters>());
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

            _signer.Init(false, key.GetParameter<ECPublicKeyParameters>());

            _signer.BlockUpdate(text, 0, text.Length);

            return _signer.VerifySignature(DerEncodeSignature(signature));
        }

        //http://stackoverflow.com/questions/37572306/verifying-ecdsa-signature-with-bouncy-castle-in-c-sharp
        private static byte[] DerEncodeSignature(byte[] signature)
        {
            var r = signature.RangeSubset(0, (signature.Length / 2));
            var s = signature.RangeSubset((signature.Length / 2), (signature.Length / 2));

            using (var stream = new MemoryStream())
            {
                var derStream = new DerOutputStream(stream);

                Asn1EncodableVector vector = new Asn1EncodableVector
                {
                    new DerInteger(new BigInteger(1, r)),
                    new DerInteger(new BigInteger(1, s))
                };
                derStream.WriteObject(new DerSequence(vector));

                return stream.ToArray();
            }
        }
    }
}
