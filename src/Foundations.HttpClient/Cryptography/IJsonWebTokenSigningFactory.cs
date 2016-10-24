namespace Foundations.HttpClient.Cryptography
{
    public interface IJsonWebTokenSigningFactory
    {
        ISigningAlgorithm GetAlgorithm(JsonWebTokenAlgorithm algorithm);
    }
}
