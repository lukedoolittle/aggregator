using System;
using System.Collections.Generic;
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
        private readonly Dictionary<string, int> _curveSignatureSizes = 
            new Dictionary<string, int>
            {
                {"P-256",  64},
                {"P-384",  96},
                {"P-521", 132}
            };
        public string SignatureMethod => _signer.AlgorithmName.ToUpper();

        private readonly ISigner _signer;
        private readonly int _rawSignatureLength;

        public EllipticCurveSigningAlgorithm(
            ISigner signer, 
            string curveName)
        {
            if (signer == null)
            {
                throw new ArgumentNullException(nameof(signer));
            }
            if (string.IsNullOrEmpty(curveName))
                throw new ArgumentException("Value cannot be null or empty.", nameof(curveName));

            _signer = signer;
            _rawSignatureLength = _curveSignatureSizes[curveName];
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

            //Bouncycastle signature verifier expects ASN.1 encoded signature. So if signature
            //is "raw" then peform DER encoding
            if (signature.Length == _rawSignatureLength)
            {
                signature = DerEncode(signature);
            }

            return _signer.VerifySignature(signature);
        }

        //http://stackoverflow.com/questions/37572306/verifying-ecdsa-signature-with-bouncy-castle-in-c-sharp
        public static byte[] DerEncode(byte[] signature)
        {
            if (signature == null) throw new ArgumentNullException(nameof(signature));

            var r = signature.RangeSubset(0, (signature.Length / 2));
            var s = signature.RangeSubset((signature.Length / 2), (signature.Length / 2));

            using (var stream = new MemoryStream())
            {
                var derStream = new DerOutputStream(stream);

                var vector = new Asn1EncodableVector
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
