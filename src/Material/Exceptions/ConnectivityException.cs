using System;

namespace Material.Exceptions
{
    public class ConnectivityException : Exception
    {
        public ConnectivityException(string message) : base(message) { }
    }
}
