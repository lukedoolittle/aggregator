using LightMock;
using SimpleCQRS.Framework.Contracts;
using SimpleCQRS.Infrastructure;

namespace Aggregator.Test.Helpers.Mocks
{
    using System.Threading.Tasks;

    public class CommandSenderMock : MockBase<ICommandSender>, ICommandSender
    {
        private object _lastCommand;

        public Task Send<T>(T command) 
            where T : Command
        {
            _lastCommand = command;
            _invoker.Invoke(a => a.Send(command));
            return Task.FromResult(0);
        }

        public void AssertSendCount<T>(int count)
             where T : Command
        {
            _context.Assert(
                a => a.Send(The<T>.IsAnyValue),
                Invoked.Exactly(count));
        }

        public void AssertMinimumSendCount<T>(int count)
             where T : Command
        {
            _context.Assert(
                a => a.Send(The<T>.IsAnyValue),
                Invoked.AtLeast(count));
        }

        public T GetLastCommand<T>()
            where T : Command
        {
            return (T)_lastCommand;
        }
    }
}
