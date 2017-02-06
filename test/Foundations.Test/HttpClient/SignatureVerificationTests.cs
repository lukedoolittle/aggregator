using System;
using System.IO;
using Foundations.Extensions;
using Foundations.HttpClient.Cryptography;
using Foundations.HttpClient.Cryptography.Discovery;
using Foundations.HttpClient.Cryptography.Enums;
using Foundations.HttpClient.Cryptography.Keys;
using Foundations.HttpClient.Extensions;
using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Math;
using Xunit;

namespace Foundations.Test.HttpClient
{
    [Trait("Category", "Continuous")]
    public class SignatureVerificationTests
    {
        private readonly IJsonWebTokenSigningFactory _factory =
            new JsonWebTokenSignerFactory();

        private readonly TestData.TestData _testData = new TestData.TestData();

        [Fact]
        public void CreateSignatureForRs256JsonWebToken()
        {
            var signatureBase = _testData.Rs256.SignatureBase;
            var expectedSignature = _testData.Rs256.Signature.UrlEncodedBase64ToBase64String();
            var privateKey = _testData.Rs256.PrivateKey;
            var algorithm = JsonWebTokenAlgorithm.RS256;

            var key = new CryptoKey(privateKey, true, StringEncoding.Base64);

            var signer = _factory.GetSigningAlgorithm(algorithm);

            var actualSignature = Convert.ToBase64String(signer.SignMessage(
                signatureBase,
                key));

            Assert.Equal(expectedSignature, actualSignature);
        }

        [Fact]
        public void CreateSignatureFoHs256JsonWebToken()
        {
            var signatureBase = _testData.Hs256.SignatureBase;
            var expectedSignature = _testData.Hs256.Signature.UrlEncodedBase64ToBase64String();
            var hashKey = _testData.Hs256.PrivateKey;
            var algorithm = JsonWebTokenAlgorithm.HS256;

            var key = new HashKey(hashKey, StringEncoding.Utf8);

            var signer = _factory.GetSigningAlgorithm(algorithm);

            var actualSignature = Convert.ToBase64String(signer.SignMessage(
                signatureBase,
                key));

            Assert.Equal(expectedSignature, actualSignature);
        }

        [Fact]
        public void VerifySignatureForRs256JsonWebToken()
        {
            var signatureBase = _testData.Rs256.SignatureBase;
            var signature = _testData.Rs256.Signature;
            var publicKey = _testData.Rs256.PublicKey;
            var algorithm = JsonWebTokenAlgorithm.RS256;

            var key = new CryptoKey(publicKey, false, StringEncoding.Base64);


            var verifier = _factory.GetVerificationAlgorithm(algorithm);

            var isVerified = verifier.VerifyMessage(
                key,
                Convert.FromBase64String(signature.UrlEncodedBase64ToBase64String()),
                signatureBase);

            Assert.True(isVerified);
        }

        [Fact]
        public void VerifySignatureForEs256JsonWebToken()
        {
            var signatureBase = _testData.Es256.SignatureBase;
            var signature = _testData.Es256.Signature;
            var publicKey = _testData.Es256.PublicKey;
            var algorithm = JsonWebTokenAlgorithm.ES256;

            var key = new CryptoKey(publicKey, false, StringEncoding.Base64);
            
            var verifier = _factory.GetVerificationAlgorithm(algorithm);

            var isVerified = verifier.VerifyMessage(
                key,
                Convert.FromBase64String(signature.UrlEncodedBase64ToBase64String()),
                signatureBase);

            Assert.True(isVerified);
        }

        [Fact]
        public void VerifySignatureForHs256JsonWebToken()
        {
            var signatureBase = _testData.Hs256.SignatureBase;
            var signature = _testData.Hs256.Signature;
            var hashKey = _testData.Hs256.PrivateKey;
            var algorithm = JsonWebTokenAlgorithm.HS256;

            var key = new HashKey(hashKey, StringEncoding.Utf8);

            var verifier = _factory.GetVerificationAlgorithm(algorithm);

            var isVerified = verifier.VerifyMessage(
                key,
                Convert.FromBase64String(signature.UrlEncodedBase64ToBase64String()),
                signatureBase);

            Assert.True(isVerified);
        }

        [Fact]
        public void VerifySignatureForRs256JsonWebTokenWithJsonWebKey()
        {
            var signatureBase = _testData.Rs256.SignatureBase;
            var signature = _testData.Rs256.Signature;
            var publicKey = _testData.Rs256.PublicKey;
            var algorithm = JsonWebTokenAlgorithm.RS256;

            var cryptoKeyParameters = new CryptoKey(publicKey, false, StringEncoding.Base64)
                .GetParameter<RsaKeyParameters>();

            var key = new JsonWebKey()
            {
                Algorithm = "RS256",
                KeyType = "RSA",
                E = Convert.ToBase64String(cryptoKeyParameters.Exponent.ToByteArray()),
                N = Convert.ToBase64String(cryptoKeyParameters.Modulus.ToByteArray())
            };

            var verifier = _factory.GetVerificationAlgorithm(algorithm);

            var isVerified = verifier.VerifyMessage(
                key.ToCryptoKey(),
                Convert.FromBase64String(signature.UrlEncodedBase64ToBase64String()),
                signatureBase);

            Assert.True(isVerified);
        }

        [Fact]
        public void VerifySignatureForEs256JsonWebTokenWithJsonWebKey()
        {
            var signatureBase = _testData.Es256.SignatureBase;
            var signature = _testData.Es256.Signature;
            var publicKey = _testData.Es256.PublicKey;
            var algorithm = JsonWebTokenAlgorithm.ES256;

            var cryptoKeyParameters = new CryptoKey(publicKey, false, StringEncoding.Base64)
                .GetParameter<ECPublicKeyParameters>();

            var key = new JsonWebKey()
            {
                Algorithm = "ES256",
                KeyType = "EC",
                CurveName = "P-256",
                X = Convert.ToBase64String(cryptoKeyParameters.Q.AffineXCoord.ToBigInteger().ToByteArray()),
                Y = Convert.ToBase64String(cryptoKeyParameters.Q.AffineYCoord.ToBigInteger().ToByteArray())
            };

            var verifier = _factory.GetVerificationAlgorithm(algorithm);

            var isVerified = verifier.VerifyMessage(
                key.ToCryptoKey(),
                Convert.FromBase64String(signature.UrlEncodedBase64ToBase64String()),
                signatureBase);

            Assert.True(isVerified);
        }

        private static byte[] DerEncode(byte[] signature)
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
