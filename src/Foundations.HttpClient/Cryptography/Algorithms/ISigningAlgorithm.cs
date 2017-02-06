using Foundations.HttpClient.Cryptography.Keys;

namespace Foundations.HttpClient.Cryptography.Algorithms
{
    public interface ISigningAlgorithm
    {
        string SignatureMethod { get; }

        byte[] SignMessage(
            string message,
            CryptoKey privateKey);
    }
}
