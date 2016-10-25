using System;

namespace Material.Exceptions
{
    public class AuthorizationException : Exception
    {
        public AuthorizationException(string message) : base(message)
        { }

        public AuthorizationException() : base()
        { }

        public AuthorizationException(string message, Exception exception) : 
            base(message, exception)
        { }
    }
}
