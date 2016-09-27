namespace Foundations.HttpClient
{
    public interface IJWTSigningFactory
    {
        IJWTSigningAlgorithm GetAlgorithm(string jwtType);
    }

    public interface IJWTSigningAlgorithm
    {
        byte[] SignText(byte[] text, string privateKey);
    }
}
