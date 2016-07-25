using System;

namespace Material.Exceptions
{
    public class BadHttpRequestException : Exception
    {
        public BadHttpRequestException(string message) : base(message)
        { }

        public BadHttpRequestException() : base()
        { }
    }
}
