using System.Runtime.Serialization;
using Foundations.HttpClient.Enums;

namespace Material.Infrastructure.Credentials
{
    [DataContract]
    public class ApiKeyCredentials : TokenCredentials
    {
        [DataMember(Name = "keyName")]
        public string KeyName { get; private set; }

        [DataMember(Name = "keyValue")]
        public string KeyValue { get; private set; }

        [DataMember(Name = "keyType")]
        public HttpParameterType KeyType { get; private set; }

        public override string ExpiresIn => "0";
        public override bool AreValidIntermediateCredentials => true;

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
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
