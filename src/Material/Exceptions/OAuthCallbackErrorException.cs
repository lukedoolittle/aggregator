using System;

namespace Material.Exceptions
{
    public class OAuthCallbackErrorException : Exception
    {
        public OAuthCallbackErrorException(string message, Exception exception) : 
            base (message, exception)
        { }

        public OAuthCallbackErrorException(string message) : base(message)
        { }

        public OAuthCallbackErrorException() : base()
        { }
    }
}
