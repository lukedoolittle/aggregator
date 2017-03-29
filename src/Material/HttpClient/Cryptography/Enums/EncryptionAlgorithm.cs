using Material.Framework.Metadata;

namespace Material.HttpClient.Cryptography.Enums
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
