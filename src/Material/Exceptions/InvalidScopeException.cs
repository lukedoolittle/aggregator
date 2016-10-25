using System;

namespace Material.Exceptions
{
    public class InvalidScopeException : Exception
    {
        public InvalidScopeException(string message) : base(message)
        { }

        public InvalidScopeException(string message, Exception exception) :
            base(message, exception)
        { }

        public InvalidScopeException() : base()
        { }
    }
}
