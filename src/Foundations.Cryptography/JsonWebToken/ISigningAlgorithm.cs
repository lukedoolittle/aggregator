namespace Foundations.Cryptography.JsonWebToken
{
    public interface ISigningAlgorithm
    {
        byte[] SignText(byte[] text, string privateKey);

        bool VerifyText(string publicKey, byte[] signature, byte[] text);
    }
}
