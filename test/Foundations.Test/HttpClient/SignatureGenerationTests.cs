using System;
using System.IO;
using System.Text;
using Foundations.Extensions;
using Foundations.HttpClient.Cryptography;
using Foundations.HttpClient.Cryptography.Enums;
using Foundations.HttpClient.Cryptography.Keys;
using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Math;
using Xunit;

namespace Foundations.Test.HttpClient
{
    [Trait("Category", "Continuous")]
    public class SignatureGenerationTests
    {
        private readonly IJsonWebTokenSigningFactory _factory =
            new JsonWebTokenSignerFactory();

        private readonly TestData.TestData _testData = new TestData.TestData();

        [Fact]
        public void SignRs256JsonWebToken()
        {
            var signatureBase = _testData.Rs256.SignatureBase;
            var expectedSignature = _testData.Rs256.Signature.ToProperBase64String();
            var privateKey = _testData.Rs256.PrivateKey;
            var algorithm = JsonWebTokenAlgorithm.RS256;

            var key = new CryptoKey(privateKey, true);

            var signatureBaseBytes = Encoding.UTF8.GetBytes(signatureBase);

            var signer = _factory.GetSigningAlgorithm(algorithm);

            var actualSignature = Convert.ToBase64String(signer.SignText(
                signatureBaseBytes,
                key));

            Assert.Equal(expectedSignature, actualSignature);
        }

        [Fact]
        public void SignHs256JsonWebToken()
        {
            var signatureBase = _testData.Hs256.SignatureBase;
            var expectedSignature = _testData.Hs256.Signature.ToProperBase64String();
            var hashKey = _testData.Hs256.PrivateKey;
            var algorithm = JsonWebTokenAlgorithm.HS256;

            var key = new HashKey(hashKey);

            var signatureBaseBytes = Encoding.UTF8.GetBytes(signatureBase);

            var signer = _factory.GetSigningAlgorithm(algorithm);

            var actualSignature = Convert.ToBase64String(signer.SignText(
                signatureBaseBytes,
                key));

            Assert.Equal(expectedSignature, actualSignature);
        }

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
