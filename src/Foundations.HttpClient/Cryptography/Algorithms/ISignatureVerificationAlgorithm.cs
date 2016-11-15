namespace Foundations.HttpClient.Cryptography.Algorithms
{
    public interface ISignatureVerificationAlgorithm
    {
        bool VerifyText(
            string modulus, 
            string publicExponent, 
            byte[] signature,
            byte[] text);
    }
}
