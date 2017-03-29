using Material.HttpClient.Cryptography.Keys;

namespace Material.HttpClient.Cryptography.Algorithms
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
