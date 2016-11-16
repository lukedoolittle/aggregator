using Foundations.Attributes;

namespace Foundations.HttpClient.Cryptography.Enums
{
    public enum EncryptionAlgorithm
    {
        [Description("RSA")]
        RSA,
        [Description("EC")]
        EllipticCurve,
        [Description("ECDSA")]
        EllipticCurveDsa
    }
}
