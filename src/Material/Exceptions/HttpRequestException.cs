using System;

namespace Material.Exceptions
{
    public class HttpRequestException : Exception
    {
        public HttpRequestException(string message) : base(message)
        { }

        public HttpRequestException() : base()
        { }

        public HttpRequestException(string message, Exception exception) :
            base(message, exception)
        { }
    }
}
