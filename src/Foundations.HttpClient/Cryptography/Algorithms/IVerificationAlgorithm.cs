
namespace Foundations.HttpClient.Cryptography.Algorithms
{
    public interface IVerificationAlgorithm
    {
        string SignatureMethod { get; }

        bool VerifyText(
            string key, 
            byte[] signature, 
            byte[] text);
    }
}
