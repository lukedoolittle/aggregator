using System;

namespace Material.Exceptions
{
    public class InvalidFlowTypeException : Exception
    {
        public InvalidFlowTypeException(string message) : base(message)
        { }

        public InvalidFlowTypeException(string message, Exception exception) : 
            base(message, exception)
        { }

        public InvalidFlowTypeException() : base()
        { }
    }
}
