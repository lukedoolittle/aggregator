using System;

namespace Aggregator.Framework.Exceptions
{
    public class BadHttpRequestException : Exception
    {
        public BadHttpRequestException(string message) : base(message)
        { }

        public BadHttpRequestException() : base()
        { }
    }
}
