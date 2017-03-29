using System;

namespace Material.Framework.Exceptions
{
    public class InvalidGrantTypeException : Exception
    {
        public InvalidGrantTypeException(string message) : base(message) { }

        public InvalidGrantTypeException() : base() { }

        public InvalidGrantTypeException(string message, Exception exception) : 
            base(message, exception) { }
    }
}
