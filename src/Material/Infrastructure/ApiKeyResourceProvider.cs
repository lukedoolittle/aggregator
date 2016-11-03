using Foundations.HttpClient.Enums;

namespace Material.Infrastructure
{
    //TODO: should override GetHashCode() for this value object
    public abstract class ApiKeyResourceProvider : ResourceProvider
    {
        public abstract string KeyName { get; }

        public abstract HttpParameterType KeyType { get; }
    }
}
