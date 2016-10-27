using Foundations.HttpClient.Enums;

namespace Material.Infrastructure.Credentials
{
    public class ApiKeyCredentials : TokenCredentials
    {
        public string KeyName { get; private set; }

        public string KeyValue { get; private set; }

        public HttpParameterType KeyType { get; private set; }

        public override bool HasValidPublicKey => true;
        public override string ExpiresIn => "0";
        public override bool AreValidIntermediateCredentials => true;

        public static ApiKeyCredentials FromProvider<TResourceProvider>(
            string apiKey)
            where TResourceProvider : ApiKeyResourceProvider, new()
        {
            var provider = new TResourceProvider();
            return new ApiKeyCredentials
            {
                KeyName = provider.KeyName,
                KeyType = provider.KeyType,
                KeyValue = apiKey
            };
        }
    }
}
