using System;

namespace Material.Exceptions
{
    public class InvalidFlowTypeException : Exception
    {
        public InvalidFlowTypeException(string message) : base(message)
        { }
    }
}
