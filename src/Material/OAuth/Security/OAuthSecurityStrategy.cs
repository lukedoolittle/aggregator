using System;
using System.Globalization;
using System.Security;
using Foundations.HttpClient.Cryptography;
using Foundations.HttpClient.Cryptography.Enums;
using Material.Contracts;

namespace Material.OAuth.Security
{
    public class OAuthSecurityStrategy : IOAuthSecurityStrategy
    {
        private readonly ICryptographicParameterRepository _repository;
        private readonly TimeSpan _cryptographicParameterTimeout;
        private readonly ICryptoStringGenerator _stringGenerator;

        public OAuthSecurityStrategy(
            ICryptographicParameterRepository repository, 
            TimeSpan cryptographicParameterTimeout,
            ICryptoStringGenerator stringGenerator)
        {
            if (repository == null) throw new ArgumentNullException(nameof(repository));
            if (stringGenerator == null) throw new ArgumentNullException(nameof(stringGenerator));

            _cryptographicParameterTimeout = cryptographicParameterTimeout;
            _repository = repository;
            _stringGenerator = stringGenerator;
        }

        public OAuthSecurityStrategy(
            ICryptographicParameterRepository repository,
            TimeSpan cryptographicParameterTimeout) : 
                this(
                    repository, 
                    cryptographicParameterTimeout, 
                    new CryptoStringGenerator())
        { }

        public string GetSecureParameter(
            string userId,
            string parameterName)
        {
            if (userId == null) throw new ArgumentNullException(nameof(userId));
            if (parameterName == null) throw new ArgumentNullException(nameof(parameterName));

            var parameterValue = GetParameter(
                userId,
                parameterName);

            if (parameterValue == null)
            {
                throw new SecurityException(
                    string.Format(
                        CultureInfo.InvariantCulture,
                        StringResources.SecurityParameterDoesNotExist,
                        parameterName,
                        userId));
            }
            else
            {
                return parameterValue;
            }
        }

        public string CreateSecureParameter(
            string userId, 
            string parameterName)
        {
            if (userId == null) throw new ArgumentNullException(nameof(userId));
            if (parameterName == null) throw new ArgumentNullException(nameof(parameterName));

            var cryptoParameter = _stringGenerator.CreateRandomString(
                32,
                CryptoStringType.Base64Alphanumeric);

            SetSecureParameter(
                userId, 
                parameterName, 
                cryptoParameter);

            return cryptoParameter;
        }

        public void SetSecureParameter(
            string userId, 
            string parameterName, 
            string parameterValue)
        {
            if (userId == null) throw new ArgumentNullException(nameof(userId));
            if (parameterName == null) throw new ArgumentNullException(nameof(parameterName));
            if (parameterValue == null) throw new ArgumentNullException(nameof(parameterValue));

            GetParameter(
                userId,
                parameterName);

            var success = _repository.TryInsertCryptographicParameterValue(
                userId, 
                parameterName, 
                parameterValue, 
                DateTimeOffset.Now);

            if (!success)
            {
                throw new SecurityException(
                    string.Format(
                        CultureInfo.InvariantCulture,
                        StringResources.SecurityParameterAlreadyExists,
                        parameterName,
                        userId));
            }
        }

        public bool IsSecureParameterValid(
            string userId, 
            string parameterName, 
            string parameterValue)
        {
            if (userId == null) throw new ArgumentNullException(nameof(userId));
            if (parameterName == null) throw new ArgumentNullException(nameof(parameterName));

            if (parameterValue == null)
            {
                return false;
            }

            return parameterValue == GetParameter(
                userId,
                parameterName);
        }

        public void ClearSecureParameters(string userId)
        {
            if (userId == null) throw new ArgumentNullException(nameof(userId));

            _repository.DeleteCryptographicParameterValues(userId);
        }

        private string GetParameter(
            string userId,
            string parameterName)
        {
            var parameterValue = _repository.GetCryptographicParameterValue(
                userId,
                parameterName);

            if (parameterValue == null)
            {
                return null;
            }
            else if (DateTimeOffset.Now - _cryptographicParameterTimeout > parameterValue.Item2)
            {
                _repository.DeleteCryptographicParameterValue(
                    userId,
                    parameterName);

                return null;
            }
            else
            {
                return parameterValue.Item1;
            }
        }
    }
}
