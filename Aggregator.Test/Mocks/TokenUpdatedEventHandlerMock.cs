using Aggregator.Domain.Events;
using Aggregator.Domain.Write;
using SimpleCQRS.Framework.Contracts;

namespace Aggregator.Test.Helpers.Mocks
{
    public class TokenUpdatedEventHandlerMock<TService> : 
        MockBase<IEventHandler<TokenUpdated<TService>>>, 
        IEventHandler<TokenUpdated<TService>>
        where TService : Service
    {
        public int Handles { get; private set; }

        public void Handle(TokenUpdated<TService> @event)
        {
            Handles++;
            _invoker.Invoke(a => a.Handle(@event));
        }
    }
}
