using System;
using System.Security.Cryptography;
using Foundations.Cryptography;
using Foundations.Cryptography.StringCreation;

namespace Material.Lite
{
    public class DotNetStringGenerator : ICryptoStringGenerator
    {
        public string CreateRandomString(
            int stringLength, 
            CryptoStringTypeEnum stringType)
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
                case CryptoStringTypeEnum.Base64AlphaNumeric:
                    return result
                        .Replace('/', '_')
                        .Replace('+', '-');
                //Note that this substitution reduces the integrity of the strong string
                //but is necessary in certain URL scenarios
                case CryptoStringTypeEnum.LowercaseAlphaNumeric:
                    return result
                        .Replace('/', 'a')
                        .Replace('+', 'b')
                        .ToLower();
                case CryptoStringTypeEnum.Base64:
                default:
                    return result;
            }
        }
    }
}
