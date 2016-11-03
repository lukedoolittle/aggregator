using System.IO;
using System.Text;
using Foundations.HttpClient.Authenticators;
using Foundations.HttpClient.Cryptography;
using Foundations.HttpClient.Serialization;
using Material.Infrastructure.Credentials;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.OpenSsl;
using Org.BouncyCastle.Security;
using Quantfabric.Test.Integration;
using Xunit;

namespace Quantfabric.Test.Material.Unit
{
    public class JsonWebTokenTests
    {
        [Fact]
        public void CreateJsonWebTokenThenVerifySignatureWithPublicKey()
        {
            var token = new JsonWebToken
            {
                Claims =
                {
                    Issuer = TestUtilities.RandomString(0, 70),
                    Scope = TestUtilities.RandomString(0, 50),
                    Audience = TestUtilities.RandomString(0, 50),
                    ExpirationTime = TestUtilities.RandomNumber(0, int.MaxValue),
                    IssuedAt = TestUtilities.RandomNumber(0, int.MaxValue)
                }
            };
            var signer = new JsonWebTokenSignerFactory().GetAlgorithm(token.Header.Algorithm);

            var serializer = new JsonSerializer();
            var header = serializer.Serialize(token.Header);
            var claims = serializer.Serialize(token.Claims);

            var signingTemplate = new OAuth2JsonWebTokenSigningTemplate(
                new JsonWebTokenSignerFactory());
            var signatureBase = signingTemplate.CreateSignatureBase(header, claims);

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

        [Fact]
        public void CreateJsonWebTokenThenVerifySignatureWithModulusAndPublicExponent()
        {
            var token = new JsonWebToken
            {
                Claims =
                {
                    Issuer = TestUtilities.RandomString(0, 70),
                    Scope = TestUtilities.RandomString(0, 50),
                    Audience = TestUtilities.RandomString(0, 50),
                    ExpirationTime = TestUtilities.RandomNumber(0, int.MaxValue),
                    IssuedAt = TestUtilities.RandomNumber(0, int.MaxValue)
                }
            };
            
            var serializer = new JsonSerializer();
            var header = serializer.Serialize(token.Header);
            var claims = serializer.Serialize(token.Claims);

            var signingTemplate = new OAuth2JsonWebTokenSigningTemplate(
                new JsonWebTokenSignerFactory());
            var signatureBase = signingTemplate.CreateSignatureBase(header, claims);

            var encodedSignatureBase = Encoding.UTF8.GetBytes(signatureBase);

            var keyPair = CreateKey();
            var privateKey = ConvertPrivateKeyToString(keyPair.Private);
            var modulus = GetModulusFromRsaKey(keyPair.Public);
            var exponent = GetExponentFromRsaKey(keyPair.Public);

            var factory = new JsonWebTokenSignerFactory();
            var verifier = factory.GetSignatureVerificationAlgorithm(token.Header.Algorithm);
            var signer = factory.GetAlgorithm(token.Header.Algorithm);

            var cipherText = signer.SignText(
                encodedSignatureBase,
                privateKey);

            Assert.True(verifier.VerifyText(
                modulus, 
                exponent, 
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

        private string GetModulusFromRsaKey(AsymmetricKeyParameter key)
        {
            var rsaKey = (RsaKeyParameters) key;
            return rsaKey.Modulus.ToString();
        }

        private string GetExponentFromRsaKey(AsymmetricKeyParameter key)
        {
            var rsaKey = (RsaKeyParameters)key;
            return rsaKey.Exponent.ToString();
        }

        private AsymmetricCipherKeyPair CreateKey()
        {
            RsaKeyPairGenerator r = new RsaKeyPairGenerator();
            r.Init(new KeyGenerationParameters(new SecureRandom(), 1024));
            return r.GenerateKeyPair();
        }
    }
}
