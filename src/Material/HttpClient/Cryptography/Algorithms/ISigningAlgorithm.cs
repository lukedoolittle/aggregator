using Material.HttpClient.Cryptography.Keys;

namespace Material.HttpClient.Cryptography.Algorithms
{
    public interface ISigningAlgorithm
    {
        string SignatureMethod { get; }

        byte[] SignMessage(
            string message,
            CryptoKey privateKey);
    }
}
