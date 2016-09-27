using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using Foundations.HttpClient;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Security;

namespace Quantfabric.Test.Material.Integration
{
    public class TestJWTSigningFactory : IJWTSigningFactory
    {
        public IJWTSigningAlgorithm GetAlgorithm(string jwtType)
        {
            return new TestJWTSigningAlgorithm();
        }
    }

    public class TestJWTSigningAlgorithm : IJWTSigningAlgorithm
    {
        public byte[] SignText(byte[] text, string privateKey)
        {
            RSAParameters rsaParameters = ConvertPKCS8ToRSAParameters(privateKey);
            var aescsp = new RSACryptoServiceProvider();
            aescsp.ImportParameters(rsaParameters);

            return aescsp.SignData(text, "SHA256");
        }

        private const string PrivateKeyPrefix = "-----BEGIN PRIVATE KEY-----";
        private const string PrivateKeySuffix = "-----END PRIVATE KEY-----";

        /// <summary>
        /// Converts the PKCS8 private key to RSA parameters. This method uses the Bouncy Castle library.
        /// </summary>
        private static RSAParameters ConvertPKCS8ToRSAParameters(string pkcs8PrivateKey)
        {
            RsaPrivateCrtKeyParameters crtParameters = ConvertPKCS8ToRsaPrivateCrtKeyParameters(pkcs8PrivateKey);
            var rp = new RSAParameters();
            rp.Modulus = crtParameters.Modulus.ToByteArrayUnsigned();
            rp.Exponent = crtParameters.PublicExponent.ToByteArrayUnsigned();
            rp.P = crtParameters.P.ToByteArrayUnsigned();
            rp.Q = crtParameters.Q.ToByteArrayUnsigned();
            rp.D = ConvertRSAParametersField(crtParameters.Exponent, rp.Modulus.Length);
            rp.DP = ConvertRSAParametersField(crtParameters.DP, rp.P.Length);
            rp.DQ = ConvertRSAParametersField(crtParameters.DQ, rp.Q.Length);
            rp.InverseQ = ConvertRSAParametersField(crtParameters.QInv, rp.Q.Length);
            return rp;
        }

        /// <summary>
        /// Converts the PKCS8 private key to RSA parameters. This method uses the Bouncy Castle library.
        /// </summary>
        private static RsaPrivateCrtKeyParameters ConvertPKCS8ToRsaPrivateCrtKeyParameters(string pkcs8PrivateKey)
        {
            var base64PrivateKey = pkcs8PrivateKey.Replace(PrivateKeyPrefix, "").Replace("\n", "")
                .Replace(PrivateKeySuffix, "");
            var privateKeyBytes = Convert.FromBase64String(base64PrivateKey);
            return (RsaPrivateCrtKeyParameters)PrivateKeyFactory.CreateKey(privateKeyBytes);
        }

        private static byte[] ConvertRSAParametersField(BigInteger n, int size)
        {
            byte[] bs = n.ToByteArrayUnsigned();

            if (bs.Length == size)
                return bs;

            if (bs.Length > size)
                throw new ArgumentException("Specified size too small", "size");

            byte[] padded = new byte[size];
            Array.Copy(bs, 0, padded, size - bs.Length, bs.Length);
            return padded;
        }
    }
}
