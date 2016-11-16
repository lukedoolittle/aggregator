using Foundations.HttpClient.Cryptography.Keys;

namespace Foundations.HttpClient.Cryptography.Algorithms
{
    public interface IVerificationAlgorithm
    {
        string SignatureMethod { get; }

        bool VerifyText(
            CryptoKey key, 
            byte[] signature, 
            byte[] text);
    }
}
