namespace Foundations.Cryptography.DigitalSignature
{
    public interface ISigningAlgorithm
    {
        string SignatureMethod { get; }

        byte[] SignText(byte[] text, string privateKey);

        bool VerifyText(string publicKey, byte[] signature, byte[] text);
    }
}
