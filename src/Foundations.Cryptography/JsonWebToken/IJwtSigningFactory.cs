using Foundations.Cryptography.DigitalSignature;

namespace Foundations.Cryptography.JsonWebToken
{
    public interface IJwtSigningFactory
    {
        ISigningAlgorithm GetAlgorithm(JwtAlgorithmEnum algorithm);
    }
}
