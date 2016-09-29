using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Foundations.Cryptography.JsonWebToken;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.OpenSsl;
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

        [Fact]
        public void VerifyJwtSignature()
        {
            var signer = new JwtSignerFactory().GetAlgorithm(JwtAlgorithmEnum.RS256);

            var signatureBase = "eyJ0eXAiOiJKV1QiLCJhbGciOiJSUzI1NiJ9.eyJpc3MiOiJhbmFseXRpY3MtYXBpQG11c2ljbm90ZXMtMTQ0MjE3LmlhbS5nc2VydmljZWFjY291bnQuY29tIiwic2NvcGUiOiJodHRwczovL3d3dy5nb29nbGVhcGlzLmNvbS9hdXRoL2FuYWx5dGljcy5yZWFkb25seSIsImF1ZCI6Imh0dHBzOi8vYWNjb3VudHMuZ29vZ2xlLmNvbS9vL29hdXRoMi90b2tlbiIsImlhdCI6MTAwLCJleHAiOjIwMH0=";
            var encodedSignatureBase = Encoding.UTF8.GetBytes(signatureBase);

            var keyPair = CreateKey();
            var privateKey = ConvertPrivateKeyToString(keyPair.Private);
            var publicKey = ConvertPublicKeyToString(keyPair.Public);

            var cipherText = signer.SignText(
                encodedSignatureBase, 
                privateKey);

            Assert.True(
                signer.VerifyText(
                    publicKey, 
                    cipherText, 
                    encodedSignatureBase));
        }

        private string ConvertPublicKeyToString(AsymmetricKeyParameter key)
        {
            TextWriter textWriter = new StringWriter();
            PemWriter pemWriter = new PemWriter(textWriter);
            pemWriter.WriteObject(key);
            pemWriter.Writer.Flush();

            return textWriter.ToString();
        }

        private string ConvertPrivateKeyToString(AsymmetricKeyParameter key)
        {
            var pkcs8Gen = new Pkcs8Generator(key);
            var pemObj = pkcs8Gen.Generate();

            TextWriter textWriter = new StringWriter();
            PemWriter pemWriter = new PemWriter(textWriter);
            pemWriter.WriteObject(pemObj);
            pemWriter.Writer.Flush();

            return textWriter.ToString();
        }

        private AsymmetricCipherKeyPair CreateKey()
        {
            RsaKeyPairGenerator r = new RsaKeyPairGenerator();
            r.Init(new KeyGenerationParameters(new SecureRandom(), 1024));
            return  r.GenerateKeyPair();
        }
    }
}
