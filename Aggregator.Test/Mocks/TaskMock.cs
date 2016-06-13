using System;
using LightMock;
using Newtonsoft.Json.Linq;
using Aggregator.Framework.Contracts;
using SimpleCQRS.Domain;
using SimpleCQRS.Framework.Contracts;

namespace Aggregator.Test.Helpers.Mocks
{
    public class TaskMock : MockBase<ITask>, ITask
    {
        private object _lastObjectPassed;
        private Func<System.Threading.Tasks.Task> _returnFunction;
        private readonly IEventPublisher _publisher;

        public int ExecuteInvocationCount { get; private set; }

        public TaskMock(IEventPublisher publisher = null)
        {
            _publisher = publisher;
            _returnFunction = () => System.Threading.Tasks.Task.FromResult(new object());
        }

        public System.Threading.Tasks.Task Execute(object parameter = null)
        {
            ExecuteInvocationCount++;
            _lastObjectPassed = parameter;

            _invoker.Invoke(a => a.Execute(parameter));
            return _returnFunction();
        }

        public TaskMock Returns(Func<System.Threading.Tasks.Task> taskToReturn)
        {
            _returnFunction = taskToReturn;
            return this;
        }  

        public TaskMock PublishEvent<TEvent>(TEvent @event, object parameter = null)
            where TEvent : Event
        {
            _context.Arrange(f => f.Execute(parameter)).Callback(() => _publisher.Publish(@event));
            return this;
        }

        public TaskMock SetExecuteException<TException>(object parameter = null)
            where TException : Exception, new()
        {
            _context.Arrange(a => a.Execute(parameter)).Throws<TException>();

            return this;
        }

        public void AssertExecuteCallsAtLeast(int count, object parameter = null)
        {
            _context.Assert(a => a.Execute(parameter), Invoked.AtLeast(count));
        }

        public void AssertExecuteCalls(int count, object parameter = null)
        {
            _context.Assert(a => a.Execute(parameter), Invoked.Exactly(count));
        }

        public void AssertExecuteCallsAnyParameter(int count)
        {
            _context.Assert(a => a.Execute(The<JObject>.IsAnyValue), Invoked.Exactly(count));
        }

        public void AssertNoExecuteCalls(object parameter = null)
        {
            _context.Assert(a => a.Execute(parameter), Invoked.Never);
        }

        public TCastType GetLastObject<TCastType>()
        {
            return (TCastType) _lastObjectPassed;
        }
    }
}
