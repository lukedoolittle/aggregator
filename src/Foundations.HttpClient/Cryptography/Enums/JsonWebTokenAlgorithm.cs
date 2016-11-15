using Foundations.Attributes;

namespace Foundations.HttpClient.Cryptography.Enums
{
    public enum JsonWebTokenAlgorithm
    {
        [Description("HS256")]
        HS256, //HMAC with SHA-256
        [Description("HS384")]
        HS384, //HMAC with SHA-384
        [Description("HS512")]
        HS512, //HMAC with SHA-512
        [Description("ES256")]
        ES256, //ECDSA with SHA-256
        [Description("ES384")]
        ES384,
        [Description("ES512")]
        ES512,
        [Description("RS256")]
        RS256, //RSA PKCS#1 signature with SHA-256
        [Description("RS384")]
        RS384, //RSA PKCS#1 signature with SHA-384
        [Description("RS512")]
        RS512, //RSA PKCS#1 signature with SHA-512
        [Description("PS256")]
        PS256, //RSA PSS signature with SHA-256
        [Description("PS384")]
        PS384, //RSA PSS signature with SHA-384
        [Description("PS512")]
        PS512 //RSA PSS signature with SHA-512
    }
}
