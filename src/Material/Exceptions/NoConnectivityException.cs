using System;

namespace Material.Exceptions
{
    public class NoConnectivityException : Exception
    {
        public NoConnectivityException(string message) : base(message) { }

        public NoConnectivityException(string message, Exception exception) : base(message, exception) { }

        public NoConnectivityException() : base() { }
    }
}
