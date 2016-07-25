using System;

namespace Aggregator.Framework.Exceptions
{
    public class CredentialsNotPopulatedException : Exception
    {
        public CredentialsNotPopulatedException(string message) : base(message) { }
    }
}
