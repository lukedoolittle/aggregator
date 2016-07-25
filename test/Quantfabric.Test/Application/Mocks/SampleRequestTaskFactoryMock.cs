using System;
using LightMock;
using Newtonsoft.Json.Linq;
using Aggregator.Framework.Contracts;

namespace Aggregator.Test.Helpers.Mocks
{
    public class SampleRequestTaskFactoryMock : 
        MockBase<ISampleRequestTaskFactory>, 
        ISampleRequestTaskFactory
    {
        public SampleRequestTaskFactoryMock()
        {
            _context.Arrange(
                a => a.GetTask(
                    The<Guid>.IsAnyValue,
                    The<JObject>.IsAnyValue,
                    The<Type>.IsAnyValue,
                    The<string>.IsAnyValue))
                .Returns(new TaskMock());
        }
        public ITask GetTask(
            Guid aggregateId, 
            JObject credentials, 
            Type requestType, 
            string lastQuery)
        {
            return _invoker.Invoke(a => 
                a.GetTask(
                    aggregateId, 
                    credentials, 
                    requestType, 
                    lastQuery));
        }

        public void AssertMinimumNumberOfInvocations(int count)
        {
            _context.Assert(
                a => a.GetTask(
                    The<Guid>.IsAnyValue,
                    The<JObject>.IsAnyValue,
                    The<Type>.IsAnyValue,
                    The<string>.IsAnyValue),
                Invoked.AtLeast(count));
        }
    }
}
