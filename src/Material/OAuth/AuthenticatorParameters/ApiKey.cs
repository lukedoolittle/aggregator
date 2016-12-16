using System;
using Foundations.HttpClient.Authenticators;
using Foundations.HttpClient.Enums;

namespace Material.OAuth.AuthenticatorParameters
{
    public class ApiKey : IAuthenticatorParameter
    {
        public string Name { get; }
        public string Value { get; }
        public HttpParameterType Type { get; }

        public ApiKey(
            string keyName, 
            string keyValue, 
            HttpParameterType keyType)
        {
            if (keyName == null) throw new ArgumentNullException(nameof(keyName));
            if (keyValue == null) throw new ArgumentNullException(nameof(keyValue));

            Name = keyName;
            Value = keyValue;
            Type = keyType;
        }
    }
}
