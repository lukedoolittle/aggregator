using Material.Framework.Enums;
using Material.Framework.Extensions;

namespace CodeGen
{
    public class BoxedApiKeyResourceProvider
    {
        public string Name { get; }
        public string Comments { get; }
        public string KeyName { get; }
        public HttpParameterType KeyType { get; }

        public BoxedApiKeyResourceProvider(
            string name,
            string comments,
            SecurityDefinition security) : this(
                name,
                comments,
                security.Name,
                security.ParameterLocation)
        {
        }

        public BoxedApiKeyResourceProvider(
            string name,
            string comments,
            string keyName,
            string keyType)
        {
            Name = name;
            Comments = comments;
            KeyName = keyName;
            KeyType = keyType.StringToEnum<HttpParameterType>();
        }
    }
}
