namespace Foundations.Cryptography.StringCreation
{
    public interface ICryptoStringGenerator
    {
        string CreateRandomString(
            int stringLength,
            CryptoStringType stringType);
    }
}
