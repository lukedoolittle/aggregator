using System;

namespace Material.Exceptions
{
    public class GrantTypeException : Exception
    {
        public GrantTypeException(string message) : base(message) { }
    }
}
