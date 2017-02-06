using Foundations.HttpClient.Cryptography.Keys;

namespace Foundations.HttpClient.Cryptography.Algorithms
{
    public interface IVerificationAlgorithm
    {
        string SignatureMethod { get; }

        bool VerifyMessage(
            CryptoKey key, 
            byte[] signature, 
            string message);
    }
}
