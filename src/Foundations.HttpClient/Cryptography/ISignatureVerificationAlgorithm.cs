namespace Foundations.HttpClient.Cryptography
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
