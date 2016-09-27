using System.Security.Cryptography;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Security;
using Xunit;

namespace Foundations.Test
{
    public class CryptographyTests
    {
        [Fact]
        public void CallingCreateCrypto16SeveralTimesProducesRandomString()
        {
            var crypto1 = Cryptography.Security.Create16CharacterCryptographicallyStrongString();
            var crypto2 = Cryptography.Security.Create16CharacterCryptographicallyStrongString();
            var crypto3 = Cryptography.Security.Create16CharacterCryptographicallyStrongString();

            Assert.Equal(16, crypto1.Length);
            Assert.Equal(16, crypto2.Length);
            Assert.Equal(16, crypto3.Length);

            Assert.NotEqual(crypto1, crypto2);
            Assert.NotEqual(crypto2, crypto3);
        }

        [Fact]
        public void CallingCreateCrypto32SeveralTimesProducesRandomString()
        {
            var crypto1 = Cryptography.Security.Create32CharacterCryptographicallyStrongString();
            var crypto2 = Cryptography.Security.Create32CharacterCryptographicallyStrongString();
            var crypto3 = Cryptography.Security.Create32CharacterCryptographicallyStrongString();

            Assert.Equal(32, crypto1.Length);
            Assert.Equal(32, crypto2.Length);
            Assert.Equal(32, crypto3.Length);

            Assert.NotEqual(crypto1, crypto2);
            Assert.NotEqual(crypto2, crypto3);
        }

        private bool VerifySignature(byte[] keyBytes, byte[] hash, byte[] ciphertext)
        {
            var asymmetricKeyParameter = PublicKeyFactory.CreateKey(keyBytes);
            var rsaKeyParameters = (RsaKeyParameters)asymmetricKeyParameter;
            var rsaParameters = new RSAParameters
            {
                Modulus = rsaKeyParameters.Modulus.ToByteArrayUnsigned(),
                Exponent = rsaKeyParameters.Exponent.ToByteArrayUnsigned()
            };
            var rsaProvider = new RSACryptoServiceProvider();
            rsaProvider.ImportParameters(rsaParameters);
            var rsaDeformatter = new RSAPKCS1SignatureDeformatter(rsaProvider);
            rsaDeformatter.SetHashAlgorithm("SHA256");
            return rsaDeformatter.VerifySignature(hash, ciphertext);
        }
    }
}
