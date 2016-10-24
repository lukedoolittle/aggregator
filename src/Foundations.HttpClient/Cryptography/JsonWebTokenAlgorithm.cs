namespace Foundations.HttpClient.Cryptography
{
    public enum JsonWebTokenAlgorithm
    {
        HS256, //HMAC with SHA-256
        HS384, //HMAC with SHA-384
        HS512, //HMAC with SHA-512
        ES256, //ECDSA with SHA-256
        ES384,
        ES512,
        RS256, //RSA PKCS#1 signature with SHA-256
        RS384, //RSA PKCS#1 signature with SHA-384
        RS512, //RSA PKCS#1 signature with SHA-512
        PS256, //RSA PSS signature with SHA-256
        PS384, //RSA PSS signature with SHA-384
        PS512 //RSA PSS signature with SHA-512
    }
}
