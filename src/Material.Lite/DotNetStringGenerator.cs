using System;
using System.Security.Cryptography;
using Foundations.HttpClient.Cryptography;

namespace Material.Lite
{
    public class DotNetStringGenerator : ICryptoStringGenerator
    {
        public string CreateRandomString(
            int stringLength, 
            CryptoStringType stringType)
        {
            var randomBytes = new byte[stringLength];

            using (RandomNumberGenerator rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(randomBytes);
            }

            var sha256 = SHA256Managed.Create();
            var result = Convert.ToBase64String(sha256.ComputeHash(randomBytes));

            switch (stringType)
            {
                case CryptoStringType.Base64Alphanumeric:
                    return result
                        .Replace('/', '_')
                        .Replace('+', '-');
                //Note that this substitution reduces the integrity of the strong string
                //but is necessary in certain URL scenarios
                case CryptoStringType.LowercaseAlphanumeric:
                    return result
                        .Replace('/', 'a')
                        .Replace('+', 'b')
                        .ToLower();
                case CryptoStringType.Base64:
                default:
                    return result;
            }
        }
    }
}
