using Foundations.Cryptography.DigitalSignature;

namespace Foundations.Cryptography.JsonWebToken
{
    public interface IJsonWebTokenSigningFactory
    {
        ISigningAlgorithm GetAlgorithm(JsonWebTokenAlgorithm algorithm);
    }
}
