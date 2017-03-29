using Material.Framework.Enums;

namespace Material.Domain.Core
{
    public abstract class ApiKeyResourceProvider : ResourceProvider
    {
        public abstract string KeyName { get; }

        public abstract HttpParameterType KeyType { get; }
    }
}
