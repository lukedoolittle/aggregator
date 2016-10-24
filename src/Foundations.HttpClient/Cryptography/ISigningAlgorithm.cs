namespace Foundations.HttpClient.Cryptography
{
    public interface ISigningAlgorithm
    {
        string SignatureMethod { get; }

        byte[] SignText(byte[] text, string privateKey);

        bool VerifyText(string publicKey, byte[] signature, byte[] text);
    }
}
