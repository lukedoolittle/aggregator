using System;

namespace Material.Exceptions
{
    public class InvalidGrantTypeException : Exception
    {
        public InvalidGrantTypeException(string message) : base(message) { }
    }
}
