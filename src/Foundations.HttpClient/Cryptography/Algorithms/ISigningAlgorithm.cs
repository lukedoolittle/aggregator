namespace Foundations.HttpClient.Cryptography.Algorithms
{
    public interface ISigningAlgorithm
    {
        string SignatureMethod { get; }

        byte[] SignText(byte[] text, string privateKey);
    }
}
