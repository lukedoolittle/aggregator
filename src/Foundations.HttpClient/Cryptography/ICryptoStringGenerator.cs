using Foundations.HttpClient.Cryptography.Enums;
using Org.BouncyCastle.Security;

namespace Foundations.HttpClient.Cryptography
{
    public interface ICryptoStringGenerator
    {
        string CreateRandomString();

        string CreateRandomString(
            int stringLength,
            CryptoStringType stringType);
    }
}
