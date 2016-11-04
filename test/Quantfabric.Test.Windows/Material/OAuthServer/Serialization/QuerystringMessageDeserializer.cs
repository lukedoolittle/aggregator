using Foundations.Http;
using Foundations.HttpClient.Serialization;

namespace Quantfabric.Test.Material.OAuthServer.Serialization
{
    public class QuerystringMessageDeserializer : IIncommingMessageDeserializer
    {
        public TMessage DeserializeMessage<TMessage>(IncomingMessage message)
        {
            return new HtmlSerializer()
                .Deserialize<TMessage>(
                    message.Uri.Query);
        }
    }
}
