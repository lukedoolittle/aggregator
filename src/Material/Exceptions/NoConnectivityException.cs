using System;

namespace Material.Exceptions
{
    public class NoConnectivityException : Exception
    {
        public NoConnectivityException(string message) : base(message) { }
    }
}
