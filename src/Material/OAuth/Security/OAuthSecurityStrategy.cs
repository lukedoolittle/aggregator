using System;
using Foundations.Cryptography;
using Material.Contracts;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Digests;

namespace Material.OAuth
{
    public class OAuthSecurityStrategy : IOAuthSecurityStrategy
    {
        private readonly ICryptographicParameterRepository _repository;
        private readonly TimeSpan _cryptographicParameterTimeout;

        public OAuthSecurityStrategy(
            ICryptographicParameterRepository repository, 
            TimeSpan cryptographicParameterTimeout)
        {
            _cryptographicParameterTimeout = cryptographicParameterTimeout;
            _repository = repository;
        }

        public string CreateOrGetSecureParameter(
            string userId, 
            string parameterName)
        {
            return GetOrSetCrypto<Sha512Digest>(
                _repository, 
                userId, 
                parameterName, 
                _cryptographicParameterTimeout);
        }

        public void SetSecureParameter(
            string userId, 
            string parameterName, 
            string parameterValue)
        {
            _repository.SetCryptographicParameterValue(
                userId, 
                parameterName, 
                parameterValue, 
                DateTimeOffset.Now);
        }

        public bool IsSecureParameterValid(
            string userId, 
            string parameterName, 
            string parameterValue)
        {
            return CheckCrypto(
                _repository, 
                userId, 
                parameterName, 
                parameterValue,
                _cryptographicParameterTimeout);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="THash">Sha512, Sha256, Sha5</typeparam>
        /// <param name="currentCryptos">List of the currently valid cryptos</param>
        /// <param name="parameterName">Name of the cryptographic parameter</param>
        /// <param name="userId">The identifier of the user submitting the request</param>
        /// <param name="timeout">Expiration time for the parameter</param>
        /// <returns></returns>
        private static string GetOrSetCrypto<THash>(
            ICryptographicParameterRepository currentCryptos,
            string userId,
            string parameterName,
            TimeSpan timeout)
            where THash : IDigest, new()
        {
            var parameterValue = currentCryptos.GetCryptographicParameterValue(
                userId, 
                parameterName);

            if (parameterValue != null &&
                DateTimeOffset.Now - timeout > parameterValue.Item2)
            {
                currentCryptos.DeleteCryptographicParameterValue(
                    userId,
                    parameterName);
                parameterValue = null;
            }

            if (parameterValue == null)
            {
                //There is a bug in the fitbit api during a token flow where a
                //state parameter with a / or a + is somehow not url encoded 
                //correctly and thus the round-tripped state does not match the given state
                //TODO: remove this forwardslash replacement when fitbit is fixed
                //OR figure out some way to modify Cryptography to return a string without
                //+s and /s
                var cryptoParameter = Security
                    .CreateCryptographicallyStrongString<THash>()
                    .Replace('/', 'a')
                    .Replace('+', 'b');

                currentCryptos.SetCryptographicParameterValue(
                    userId,
                    parameterName,
                    cryptoParameter,
                    DateTimeOffset.Now);

                return cryptoParameter;
            }

            return parameterValue.Item1;
        }

        /// <summary>
        /// Checks given crypto list and determines if given secure parameter is valid
        /// </summary>
        /// <param name="currentCryptos">List of the currently valid cryptos</param>
        /// <param name="parameterName">Name of the cryptographic parameter</param>
        /// <param name="parameterValue">Returned value of the cryptographic parameter</param>
        /// <param name="userId">The identifier of the user submitting the request</param>
        /// <param name="timeout">Expiration time for the parameter</param>
        /// <returns>False if the parameter is incorrect or expected and missing, True otherwise</returns>
        protected static bool CheckCrypto(
            ICryptographicParameterRepository currentCryptos,
            string userId,
            string parameterName,
            string parameterValue,
            TimeSpan timeout)
        {
            var expectedParameterValue = currentCryptos.GetCryptographicParameterValue(
                userId,
                parameterName);

            if (expectedParameterValue == null)
            {
                return false;
            }
            else if (DateTimeOffset.Now - timeout > expectedParameterValue.Item2)
            {
                currentCryptos.DeleteCryptographicParameterValue(
                    userId,
                    parameterName);

                return false;
            }
            else
            {
                return parameterValue == expectedParameterValue.Item1;
            }
        }

    }
}
