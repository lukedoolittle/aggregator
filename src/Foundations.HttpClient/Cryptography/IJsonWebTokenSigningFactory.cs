using Foundations.HttpClient.Cryptography.Algorithms;
using Foundations.HttpClient.Cryptography.Enums;

namespace Foundations.HttpClient.Cryptography
{
    public interface IJsonWebTokenSigningFactory
    {
        ISigningAlgorithm GetSigningAlgorithm(JsonWebTokenAlgorithm algorithm);
        IVerificationAlgorithm GetVerificationAlgorithm(JsonWebTokenAlgorithm algorithm);
    }
}
