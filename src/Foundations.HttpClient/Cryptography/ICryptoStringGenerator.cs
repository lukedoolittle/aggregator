using Foundations.HttpClient.Cryptography.Enums;

namespace Foundations.HttpClient.Cryptography
{
    public interface ICryptoStringGenerator
    {
        string CreateRandomString(
            int stringLength,
            CryptoStringType stringType);
    }
}
