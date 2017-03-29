using System;
using System.Globalization;
using System.Net;
using Material.Framework.Enums;
using Material.Framework.Extensions;
using Material.HttpClient.Authenticators;

namespace Material.Workflow.AuthenticatorParameters
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

            if (keyName == OAuth2Parameter.BearerHeader.EnumToString() && 
                keyType == HttpParameterType.Header)
            {
                Name = HttpRequestHeader.Authorization.ToString();
                Value = string.Format(
                    CultureInfo.InvariantCulture,
                    "{0} {1}",
                    OAuth2Parameter.BearerHeader.EnumToString(),
                    keyValue);
                Type = keyType;
            }
            else
            {
                Name = keyName;
                Value = keyValue;
                Type = keyType;
            }
        }
    }
}
