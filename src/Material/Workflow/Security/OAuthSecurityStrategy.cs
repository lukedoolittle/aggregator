using System;
using System.Globalization;
using System.Security;
using Material.Contracts;
using Material.HttpClient.Cryptography;
using Material.HttpClient.Cryptography.Enums;

namespace Material.Workflow.Security
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
            string requestId,
            string parameterName)
        {
            if (requestId == null) throw new ArgumentNullException(nameof(requestId));
            if (parameterName == null) throw new ArgumentNullException(nameof(parameterName));

            var parameterValue = GetParameter(
                requestId,
                parameterName);

            if (parameterValue == null)
            {
                throw new SecurityException(
                    string.Format(
                        CultureInfo.InvariantCulture,
                        StringResources.SecurityParameterDoesNotExist,
                        parameterName,
                        requestId));
            }
            else
            {
                return parameterValue;
            }
        }

        public string CreateSecureParameter(
            string requestId, 
            string parameterName)
        {
            if (requestId == null) throw new ArgumentNullException(nameof(requestId));
            if (parameterName == null) throw new ArgumentNullException(nameof(parameterName));

            var cryptoParameter = _stringGenerator.CreateRandomString(
                32,
                CryptoStringType.Base64Alphanumeric);

            SetSecureParameter(
                requestId, 
                parameterName, 
                cryptoParameter);

            return cryptoParameter;
        }

        public void SetSecureParameter(
            string requestId, 
            string parameterName, 
            string parameterValue)
        {
            if (requestId == null) throw new ArgumentNullException(nameof(requestId));
            if (parameterName == null) throw new ArgumentNullException(nameof(parameterName));
            if (parameterValue == null) throw new ArgumentNullException(nameof(parameterValue));

            GetParameter(
                requestId,
                parameterName);

            var success = _repository.TryInsertCryptographicParameterValue(
                requestId, 
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
                        requestId));
            }
        }

        public bool IsSecureParameterValid(
            string requestId, 
            string parameterName, 
            string parameterValue)
        {
            if (requestId == null) throw new ArgumentNullException(nameof(requestId));
            if (parameterName == null) throw new ArgumentNullException(nameof(parameterName));

            if (parameterValue == null)
            {
                return false;
            }

            return parameterValue == GetParameter(
                requestId,
                parameterName);
        }

        public void ClearSecureParameters(string requestId)
        {
            if (requestId == null) throw new ArgumentNullException(nameof(requestId));

            _repository.DeleteCryptographicParameterValues(requestId);
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
