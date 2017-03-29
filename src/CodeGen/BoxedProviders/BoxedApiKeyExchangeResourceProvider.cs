using System;
using Material.Framework.Enums;
using Material.Framework.Extensions;

namespace CodeGen
{
    public class BoxedApiKeyExchangeResourceProvider
    {
        public string Name { get; }
        public string Comments { get; }

        public string KeyName { get; }
        public HttpParameterType KeyType { get; }
        public Uri TokenUrl { get; }
        public string TokenName { get; }

        public BoxedApiKeyExchangeResourceProvider(
            string name,
            string comments,
            SecurityDefinition security) : this(
                name,
                comments,
                security.KeyName,
                security.ParameterLocation,
                security.TokenUrl,
                security.Name)
        {
        }

        public BoxedApiKeyExchangeResourceProvider(
            string name,
            string comments,
            string keyName,
            string keyType,
            string tokenUrl,
            string tokenName)
        {
            Name = name;
            Comments = comments;
            KeyName = keyName;
            KeyType = keyType.StringToEnum<HttpParameterType>();
            TokenUrl = new Uri(tokenUrl);
            TokenName = tokenName;
        }
    }
}
