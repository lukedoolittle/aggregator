using System;

namespace Aggregator.Framework.Exceptions
{
    public class CredentialsDoNotExistException : Exception
    {
        public CredentialsDoNotExistException(string message) : base(message) { }
    }
}
