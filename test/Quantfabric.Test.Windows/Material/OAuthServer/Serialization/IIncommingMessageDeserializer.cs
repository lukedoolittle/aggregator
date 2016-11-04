using Foundations.Http;

namespace Quantfabric.Test.Material.OAuthServer.Serialization
{
    public interface IIncommingMessageDeserializer
    {
        TMessage DeserializeMessage<TMessage>(IncomingMessage message);
    }
}
