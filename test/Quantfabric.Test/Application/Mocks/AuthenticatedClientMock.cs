using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LightMock;
using Newtonsoft.Json.Linq;
using Aggregator.Domain.Write;
using Aggregator.Framework.Contracts;
using Material.Contracts;

namespace Aggregator.Test.Helpers.Mocks
{
    public class AuthenticatedClientMock<TToken> : 
        MockBase<IRequestClient>,
        IRequestClient
    {
        private TToken _lastToken;
        private Exception _exceptionToThrow;

        public AuthenticatedClientMock<TToken> SetReturnValue(
            IEnumerable<Tuple<DateTimeOffset, JObject>> returns)
        {
            _context.Arrange(
                a=>a.GetDataPoints(
                    The<string>.IsAnyValue))
                    .Returns(System.Threading.Tasks.Task.Run(() => returns));
            return this;
        }

        public AuthenticatedClientMock<TToken> SetException<TException>()
            where TException : Exception, new()
        {
            _exceptionToThrow = new TException();

            return this;
        }

        public void SetCredentials(object credentials)
        {
            _lastToken = (TToken) credentials;
        }

        public void AssertMinimumNumberOfInvocations(int count)
        {
            _context.Assert(
                a=>a.GetDataPoints(The<string>.IsAnyValue), 
                Invoked.AtLeast(count));
        }

        public TToken LastToken => _lastToken;

        public Task<IEnumerable<Tuple<DateTimeOffset, JObject>>> GetDataPoints(string recencyValue)
        {
            if (_exceptionToThrow != null)
            {
                return System.Threading.Tasks.Task.Run<IEnumerable<Tuple<DateTimeOffset, JObject>>>(() =>
                {
                    throw _exceptionToThrow;
#pragma warning disable 162 //this is necessary to get the correct type of the lambda
                    return new List<Tuple<DateTimeOffset, JObject>>().AsEnumerable();
#pragma warning restore 162
                });
            }
            else
            {
                return _invoker.Invoke(a => a.GetDataPoints(recencyValue));
            }
        }
    }
}
