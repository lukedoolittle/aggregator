using Foundations.Http;
using Material.Framework.Serialization;

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
