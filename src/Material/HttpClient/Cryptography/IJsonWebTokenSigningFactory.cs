using Material.HttpClient.Cryptography.Algorithms;
using Material.HttpClient.Cryptography.Enums;

namespace Material.HttpClient.Cryptography
{
    public interface IJsonWebTokenSigningFactory
    {
        ISigningAlgorithm GetSigningAlgorithm(JsonWebTokenAlgorithm algorithm);
        IVerificationAlgorithm GetVerificationAlgorithm(JsonWebTokenAlgorithm algorithm);
    }
}
