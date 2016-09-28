namespace Foundations.Cryptography.JsonWebToken
{
    public interface IJwtSigningFactory
    {
        ISigningAlgorithm GetAlgorithm(JwtAlgorithmEnum algorithm);
    }
}
