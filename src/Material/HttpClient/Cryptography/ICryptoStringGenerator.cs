using Material.HttpClient.Cryptography.Enums;

namespace Material.HttpClient.Cryptography
{
    public interface ICryptoStringGenerator
    {
        string CreateRandomString();

        string CreateRandomString(
            int stringLength,
            CryptoStringType stringType);
    }
}
