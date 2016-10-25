using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Foundations.HttpClient.Authenticators;
using Foundations.HttpClient.Cryptography;
using Foundations.HttpClient.Request;
using Newtonsoft.Json.Linq;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Security;
using Xunit;

namespace Foundations.Test
{
    public class JsonWebTokenTests
    {
        [Fact]
        public void CreateSignatureBaseCorrectlyCreatesSignature()
        {
            var expected = "eyJ0eXAiOiJKV1QiLCJhbGciOiJSUzI1NiJ9.eyJpc3MiOiJhbmFseXRpY3MtYXBpQG11c2ljbm90ZXMtMTQ0MjE3LmlhbS5nc2VydmljZWFjY291bnQuY29tIiwic2NvcGUiOiJodHRwczovL3d3dy5nb29nbGVhcGlzLmNvbS9hdXRoL2FuYWx5dGljcy5yZWFkb25seSIsImF1ZCI6Imh0dHBzOi8vYWNjb3VudHMuZ29vZ2xlLmNvbS9vL29hdXRoMi90b2tlbiIsImlhdCI6MTAwLCJleHAiOjIwMH0=";

            var token = new JsonWebToken
            {
                Claims =
                {
                    Issuer = "analytics-api@musicnotes-144217.iam.gserviceaccount.com",
                    Scope = "https://www.googleapis.com/auth/analytics.readonly",
                    Audience = "https://accounts.google.com/o/oauth2/token",
                    ExpirationTime = 200,
                    IssuedAt = 100
                }
            };

            var template = new OAuth2JsonWebTokenSigningTemplate(token, new JsonWebTokenSignerFactory());

            var actual = template.CreateSignatureBase();

            Assert.Equal(expected, actual);
        }




        //[Fact]
        //public void CreateSignatureWithGivenBaseString()
        //{
        //    var signatureBase = "eyJhbGciOiJSUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiI3NjEzMjY3OTgwNjktcjVtbGpsbG4xcmQ0bHJiaGc3NWVmZ2lncDM2bTc4ajVAZGV2ZWxvcGVyLmdzZXJ2aWNlYWNjb3VudC5jb20iLCJzY29wZSI6Imh0dHBzOi8vd3d3Lmdvb2dsZWFwaXMuY29tL2F1dGgvcHJlZGljdGlvbiIsImF1ZCI6Imh0dHBzOi8vd3d3Lmdvb2dsZWFwaXMuY29tL29hdXRoMi92NC90b2tlbiIsImV4cCI6MTMyODU1NDM4NSwiaWF0IjoxMzI4NTUwNzg1fQ";

        //    var token = new JsonWebToken
        //    {
        //        Claims =
        //        {
        //            Issuer = "761326798069-r5mljlln1rd4lrbhg75efgigp36m78j5@developer.gserviceaccount.com",
        //            Scope = "https://www.googleapis.com/auth/prediction",
        //            Audience = "https://www.googleapis.com/oauth2/v4/token",
        //            ExpirationTime = 1328554385,
        //            IssuedAt = 1328550785
        //        }
        //    };

        //    var template = new OAuth2JWTSigningTemplate(token);

        //    var actual = template.CreateSignature(signatureBase, RSATestData.PrivateKeyPem);

        //    var resultingJWT = $"{signatureBase}.{actual}";

        //    Assert.NotEmpty(Decode(resultingJWT, RSATestData.PublicKey));
        //}

        //[Fact]
        //public void CreateJsonWebTokenFromRandomData()
        //{
        //    var token = new JsonWebToken
        //    {
        //        Claims =
        //        {
        //            Issuer = RandomString(RandomNumber(0, 70)),
        //            Scope = RandomString(RandomNumber(0, 50)),
        //            Audience = RandomString(RandomNumber(0, 50)),
        //            ExpirationTime = RandomNumber(0, int.MaxValue),
        //            IssuedAt = RandomNumber(0, int.MaxValue),
        //        }
        //    };

        //    var template = new OAuth2JWTSigningTemplate(token);

        //    var @base = template.CreateSignatureBase();
        //    var signature = template.CreateSignature(@base, RSATestData.PrivateKeyPem);
        //    var jwt = template.CreateJsonWebToken(signature);

        //    Assert.NotEmpty(Decode(jwt, RSATestData.PublicKey));
        //}

        //[Fact]
        //public void CreateJsonWebTokenMultipleTimes()
        //{
        //    for (var i = 0; i < 20; i++)
        //    {
        //        CreateJsonWebTokenFromRandomData();
        //    }
        //}

        private static Random random = new Random();

        private static int RandomNumber(int start, int end)
        {
            return random.Next(start, end);
        }

        private static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-.@/";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        //Taken from
        //http://codingstill.com/2016/01/verify-jwt-token-signed-with-rs256-using-the-public-key/
        private static string Decode(string token, string key, bool verify = true)
        {
            string[] parts = token.Split('.');
            string header = parts[0];
            string payload = parts[1];
            byte[] crypto = Base64UrlDecode(parts[2]);

            string headerJson = Encoding.UTF8.GetString(Base64UrlDecode(header));
            JObject headerData = JObject.Parse(headerJson);

            string payloadJson = Encoding.UTF8.GetString(Base64UrlDecode(payload));
            JObject payloadData = JObject.Parse(payloadJson);

            if (verify)
            {
                var keyBytes = Convert.FromBase64String(key); // your key here

                AsymmetricKeyParameter asymmetricKeyParameter = PublicKeyFactory.CreateKey(keyBytes);
                RsaKeyParameters rsaKeyParameters = (RsaKeyParameters)asymmetricKeyParameter;
                RSAParameters rsaParameters = new RSAParameters();
                rsaParameters.Modulus = rsaKeyParameters.Modulus.ToByteArrayUnsigned();
                rsaParameters.Exponent = rsaKeyParameters.Exponent.ToByteArrayUnsigned();
                RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
                rsa.ImportParameters(rsaParameters);

                SHA256 sha256 = SHA256.Create();
                byte[] hash = sha256.ComputeHash(Encoding.UTF8.GetBytes(parts[0] + '.' + parts[1]));

                RSAPKCS1SignatureDeformatter rsaDeformatter = new RSAPKCS1SignatureDeformatter(rsa);
                rsaDeformatter.SetHashAlgorithm("SHA256");
                if (!rsaDeformatter.VerifySignature(hash, FromBase64Url(parts[2])))
                    throw new ApplicationException(string.Format("Invalid signature"));
            }

            return payloadData.ToString();
        }

        private static byte[] FromBase64Url(string base64Url)
        {
            string padded = base64Url.Length % 4 == 0
                ? base64Url : base64Url + "====".Substring(base64Url.Length % 4);
            string base64 = padded.Replace("_", "/")
                                    .Replace("-", "+");
            return Convert.FromBase64String(base64);
        }

        // from JWT spec
        private static byte[] Base64UrlDecode(string input)
        {
            var output = input;
            output = output.Replace('-', '+'); // 62nd char of encoding
            output = output.Replace('_', '/'); // 63rd char of encoding
            switch (output.Length % 4) // Pad with trailing '='s
            {
                case 0: break; // No pad chars in this case
                case 1: output += "==="; break; // Three pad chars
                case 2: output += "=="; break; // Two pad chars
                case 3: output += "="; break; // One pad char
                default: throw new System.Exception("Illegal base64url string!");
            }
            var converted = Convert.FromBase64String(output); // Standard base64 decoder
            return converted;
        }

    }
}
