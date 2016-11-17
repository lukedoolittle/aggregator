using Foundations.HttpClient.Enums;

namespace Material.Infrastructure
{
    public abstract class ApiKeyResourceProvider : ResourceProvider
    {
        public abstract string KeyName { get; }

        public abstract HttpParameterType KeyType { get; }
    }
}
