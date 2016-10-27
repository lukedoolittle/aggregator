using System;
using Foundations.HttpClient.Enums;

namespace Foundations.HttpClient.Authenticators
{
    public class ApiKeyProtectedResource : IAuthenticator
    {
        private readonly string _keyName;
        private readonly string _keyValue;
        private readonly HttpParameterType _keyType;

        public ApiKeyProtectedResource(
            string keyName, 
            string keyValue, 
            HttpParameterType keyType)
        {
            _keyName = keyName;
            _keyValue = keyValue;
            _keyType = keyType;
        }

        public void Authenticate(HttpRequestBuilder requestBuilder)
        {
            if (requestBuilder == null) throw new ArgumentNullException(nameof(requestBuilder));

            if (_keyType == HttpParameterType.Header)
            {
                requestBuilder.Header(
                    _keyName,
                    _keyValue);
            }
            else
            {
                requestBuilder.Parameter(
                    _keyName,
                    _keyValue);
            }
        }
    }
}
