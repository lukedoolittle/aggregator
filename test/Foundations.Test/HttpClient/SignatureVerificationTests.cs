using System;
using System.Text;
using Foundations.Extensions;
using Foundations.HttpClient.Cryptography;
using Foundations.HttpClient.Cryptography.Enums;
using Foundations.HttpClient.Cryptography.Keys;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Security;
using Quantfabric.Test.Helpers;
using Xunit;

namespace Foundations.Test.HttpClient
{
    [Trait("Category", "Continuous")]
    public class SignatureVerificationTests
    {
        private readonly IJsonWebTokenSigningFactory _factory =
            new JsonWebTokenSignerFactory();
        private readonly Randomizer _randomizer =
            new Randomizer();

        #region Golden Data

        //Generated with
        //https://kjur.github.io/jsrsasign/tool_jwt.html

        [Fact]
        public void VerifyRs256JsonWebToken()
        {
            var signatureBase = "eyJhbGciOiJSUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJodHRwczovL2p3dC1pZHAuZXhhbXBsZS5jb20iLCJzdWIiOiJtYWlsdG86bWlrZUBleGFtcGxlLmNvbSIsIm5iZiI6MTQ3OTc0Njc1NCwiZXhwIjoxNDc5NzUwMzU0LCJpYXQiOjE0Nzk3NDY3NTQsImp0aSI6ImlkMTIzNDU2IiwidHlwIjoiaHR0cHM6Ly9leGFtcGxlLmNvbS9yZWdpc3RlciIsImF1ZCI6WyJodHRwOi8vZm9vMS5jb20iLCJodHRwOi8vZm9vMi5jb20iXX0";
            var signature = "RGiBlnEY9Kei6tAmY0WkmrWy4S9BJjirLl8fvg-cfPQqFax2CE_6i-xVDN9n-kuuJYmbeHzIS7n1Jz5_Cf299QOefa-oqSr3f_apq7QduEbKhhGL-j3BcShQ4yYSahmSwzYC8_OlxeyyLTj-w6BFfWlQo_UIbsO-gZ5xm300-OwAxMhcWUM_rcvwk1--pz-Wohld703Jf-5NSpbtVgwmpqw0wtokQ9batsnrWMHFjHMZqLj1HUDwIg-THWQmdn_ea_rU9ZQeGW-WBWH2Lt9LBqtB5vgK5wLlwQ1XPw9EZWQgVdvY_YeAlx73NUueYk3gd8uAlKWawzBKTvoWXXtyZQ";
            var publicKey = @"MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEA33TqqLR3eeUmDtHS89qF3p4MP7Wfqt2Zjj3lZjLjjCGDvwr9cJNlNDiuKboODgUiT4ZdPWbOiMAfDcDzlOxA04DDnEFGAf+kDQiNSe2ZtqC7bnIc8+KSG/qOGQIVaay4Ucr6ovDkykO5Hxn7OU7sJp9TP9H0JH8zMQA6YzijYH9LsupTerrY3U6zyihVEDXXOv08vBHk50BMFJbE9iwFwnxCsU5+UZUZYw87Uu0n4LPFS9BT8tUIvAfnRXIEWCha3KbFWmdZQZlyrFw0buUEf0YN3/Q0auBkdbDR/ES2PbgKTJdkjc/rEeM0TxvOUf7HuUNOhrtAVEN1D5uuxE1WSwIDAQAB";
            var algorithm = JsonWebTokenAlgorithm.RS256;

            var publicKeyParameters = (RsaKeyParameters)PublicKeyFactory.CreateKey(Convert.FromBase64String(publicKey));
            var key = new RsaCryptoKey(publicKeyParameters);

            var signatureBaseBytes = Encoding.UTF8.GetBytes(signatureBase);

            var verifier = _factory.GetVerificationAlgorithm(algorithm);

            var isVerified = verifier.VerifyText(
                key,
                Convert.FromBase64String(signature.ToProperBase64String()),
                signatureBaseBytes);

            Assert.True(isVerified);
        }

        [Fact]
        public void VerifyEs256JsonWebToken()
        {
            var signatureBase = "eyJhbGciOiJFUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJodHRwczovL2p3dC1pZHAuZXhhbXBsZS5jb20iLCJzdWIiOiJtYWlsdG86bWlrZUBleGFtcGxlLmNvbSIsIm5iZiI6MTQ3OTUzMjM0MSwiZXhwIjoxNDc5NTM1OTQxLCJpYXQiOjE0Nzk1MzIzNDEsImp0aSI6ImlkMTIzNDU2IiwidHlwIjoiaHR0cHM6Ly9leGFtcGxlLmNvbS9yZWdpc3RlciIsImF1ZCI6WyJodHRwOi8vZm9vMS5jb20iLCJodHRwOi8vZm9vMi5jb20iXX0";
            var signature = "aGHSDpqHqGuG89OJCapCVBYvkpStCra8ZD4py02wGf7dPiC6mEdquE2YEGuYcjMKlNOR_0lwzpuNx0xoSmr81A";
            var publicKey = @"MFkwEwYHKoZIzj0CAQYIKoZIzj0DAQcDQgAEoBUyo8CQAFPeYPvv78ylh5MwFZjTCLQeb042TjiMJxG+9DLFmRSMlBQ9T/RsLLc+PmpB1+7yPAR+oR5gZn3kJQ==";
            var algorithm = JsonWebTokenAlgorithm.ES256;

            var publicKeyParameters = (ECPublicKeyParameters) PublicKeyFactory.CreateKey(
                Convert.FromBase64String(publicKey));
            var key = new EcdsaCryptoKey(publicKeyParameters, "P-256");
            var signatureBaseBytes = Encoding.UTF8.GetBytes(signatureBase);

            var verifier = _factory.GetVerificationAlgorithm(algorithm);

            var isVerified = verifier.VerifyText(
                key,
                Convert.FromBase64String(signature.ToProperBase64String()),
                signatureBaseBytes);

            Assert.True(isVerified);
        }

        [Fact]
        public void VerifyHs256JsonWebToken()
        {
            var signatureBase = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJodHRwczovL2p3dC1pZHAuZXhhbXBsZS5jb20iLCJzdWIiOiJtYWlsdG86bWlrZUBleGFtcGxlLmNvbSIsIm5iZiI6MTQ3OTc2NjIwOSwiZXhwIjoxNDc5NzY5ODA5LCJpYXQiOjE0Nzk3NjYyMDksImp0aSI6ImlkMTIzNDU2IiwidHlwIjoiaHR0cHM6Ly9leGFtcGxlLmNvbS9yZWdpc3RlciIsImF1ZCI6WyJodHRwOi8vZm9vMS5jb20iLCJodHRwOi8vZm9vMi5jb20iXX0";
            var signature = "TJuhIls03ldiNcv45baPzqPk_sUxoCUrJAkRdSZlU70";
            var hashKey = "ABCD";
            var algorithm = JsonWebTokenAlgorithm.HS256;

            var key = new HashKey(hashKey);

            var signatureBaseBytes = Encoding.UTF8.GetBytes(signatureBase);


            var mySignature = _factory.GetSigningAlgorithm(algorithm).SignText(signatureBaseBytes, key);
            var actualSignature = Convert.FromBase64String(signature.ToProperBase64String());


            var verifier = _factory.GetVerificationAlgorithm(algorithm);

            var isVerified = verifier.VerifyText(
                key,
                Convert.FromBase64String(signature.ToProperBase64String()),
                signatureBaseBytes);

            Assert.True(isVerified);
        }

        #endregion Golden Data

        #region Generated Data


        [Fact]
        public void GenerateAndVerifyRs256JsonWebToken()
        {
            var keyPair = RsaCryptoKeyPair.Create(1024);
            var plaintext = _randomizer.RandomString(20, 40);
            var algorithm = JsonWebTokenAlgorithm.RS256;

            var bytes = Encoding.UTF8.GetBytes(plaintext);
            var signer = _factory.GetSigningAlgorithm(algorithm);
            var verifier = _factory.GetVerificationAlgorithm(algorithm);

            var signature = signer.SignText(
                bytes,
                keyPair.Private);

            var isVerified = verifier.VerifyText(
                keyPair.Public,
                signature,
                bytes);

            Assert.True(isVerified);
        }

        [Fact]
        public void GenerateAndVerifyRs384JsonWebToken()
        {
            var keyPair = RsaCryptoKeyPair.Create(1024);
            var plaintext = _randomizer.RandomString(20, 40);
            var algorithm = JsonWebTokenAlgorithm.RS384;

            var bytes = Encoding.UTF8.GetBytes(plaintext);
            var signer = _factory.GetSigningAlgorithm(algorithm);
            var verifier = _factory.GetVerificationAlgorithm(algorithm);

            var signature = signer.SignText(
                bytes,
                keyPair.Private);

            var isVerified = verifier.VerifyText(
                keyPair.Public,
                signature,
                bytes);

            Assert.True(isVerified);
        }

        [Fact]
        public void GenerateAndVerifyRs512JsonWebToken()
        {
            var keyPair = RsaCryptoKeyPair.Create(1024);
            var plaintext = _randomizer.RandomString(20, 40);
            var algorithm = JsonWebTokenAlgorithm.RS384;

            var bytes = Encoding.UTF8.GetBytes(plaintext);
            var signer = _factory.GetSigningAlgorithm(algorithm);
            var verifier = _factory.GetVerificationAlgorithm(algorithm);

            var signature = signer.SignText(
                bytes,
                keyPair.Private);

            var isVerified = verifier.VerifyText(
                keyPair.Public,
                signature,
                bytes);

            Assert.True(isVerified);
        }

        [Fact]
        public void GenerateAndVerifyEs256JsonWebToken()
        {
            var curveName = "P-256";
            var keyPair = EcdsaCryptoKeyPair.Create(curveName);
            var plaintext = _randomizer.RandomString(20, 40);
            var algorithm = JsonWebTokenAlgorithm.ES256;

            var bytes = Encoding.UTF8.GetBytes(plaintext);
            var signer = _factory.GetSigningAlgorithm(algorithm);
            var verifier = _factory.GetVerificationAlgorithm(algorithm);

            var signature = signer.SignText(
                bytes,
                keyPair.Private);

            var isVerified = verifier.VerifyText(
                keyPair.Public,
                signature,
                bytes);

            Assert.True(isVerified);
        }

        [Fact]
        public void GenerateAndVerifyEs384JsonWebToken()
        {
            var curveName = "P-256";
            var keyPair = EcdsaCryptoKeyPair.Create(curveName);
            var plaintext = _randomizer.RandomString(20, 40);
            var algorithm = JsonWebTokenAlgorithm.ES384;

            var bytes = Encoding.UTF8.GetBytes(plaintext);
            var signer = _factory.GetSigningAlgorithm(algorithm);
            var verifier = _factory.GetVerificationAlgorithm(algorithm);

            var signature = signer.SignText(
                bytes,
                keyPair.Private);

            var isVerified = verifier.VerifyText(
                keyPair.Public,
                signature,
                bytes);

            Assert.True(isVerified);
        }

        [Fact]
        public void GenerateAndVerifyEs512JsonWebToken()
        {
            var curveName = "P-256";
            var keyPair = EcdsaCryptoKeyPair.Create(curveName);
            var plaintext = _randomizer.RandomString(20, 40);
            var algorithm = JsonWebTokenAlgorithm.ES512;

            var bytes = Encoding.UTF8.GetBytes(plaintext);
            var signer = _factory.GetSigningAlgorithm(algorithm);
            var verifier = _factory.GetVerificationAlgorithm(algorithm);

            var signature = signer.SignText(
                bytes,
                keyPair.Private);

            var isVerified = verifier.VerifyText(
                keyPair.Public,
                signature,
                bytes);

            Assert.True(isVerified);
        }

        [Fact]
        public void GenerateAndVerifyHs256JsonWebToken()
        {
            var key = _randomizer.RandomString(200);
            var plaintext = _randomizer.RandomString(20, 40);
            var algorithm = JsonWebTokenAlgorithm.HS256;

            var bytes = Encoding.UTF8.GetBytes(plaintext);
            var signer = _factory.GetSigningAlgorithm(algorithm);
            var verifier = _factory.GetVerificationAlgorithm(algorithm);

            var signature = signer.SignText(
                bytes,
                new HashKey(key));

            var isVerified = verifier.VerifyText(
                new HashKey(key),
                signature,
                bytes);

            Assert.True(isVerified);
        }

        public void GenerateAndVerifyHs384JsonWebToken()
        {
            var key = _randomizer.RandomString(200);
            var plaintext = _randomizer.RandomString(20, 40);
            var algorithm = JsonWebTokenAlgorithm.HS384;

            var bytes = Encoding.UTF8.GetBytes(plaintext);
            var signer = _factory.GetSigningAlgorithm(algorithm);
            var verifier = _factory.GetVerificationAlgorithm(algorithm);

            var signature = signer.SignText(
                bytes,
                new HashKey(key));

            var isVerified = verifier.VerifyText(
                new HashKey(key),
                signature,
                bytes);

            Assert.True(isVerified);
        }

        public void GenerateAndVerifyHs512JsonWebToken()
        {
            var key = _randomizer.RandomString(200);
            var plaintext = _randomizer.RandomString(20, 40);
            var algorithm = JsonWebTokenAlgorithm.HS512;

            var bytes = Encoding.UTF8.GetBytes(plaintext);
            var signer = _factory.GetSigningAlgorithm(algorithm);
            var verifier = _factory.GetVerificationAlgorithm(algorithm);

            var signature = signer.SignText(
                bytes,
                new HashKey(key));

            var isVerified = verifier.VerifyText(
                new HashKey(key),
                signature,
                bytes);

            Assert.True(isVerified);
        }

        #endregion Generated Data

    }
}
