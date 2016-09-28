namespace Foundations.Cryptography.JsonWebToken
{
    public interface ISigningAlgorithm
    {
        byte[] SignText(byte[] text, string privateKey);
    }
}
