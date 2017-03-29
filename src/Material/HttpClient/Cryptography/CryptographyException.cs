using System;

namespace Material.HttpClient.Cryptography
{
    public class CryptographyException : Exception
    {
        public CryptographyException(string message) : base(message)
        { }

        public CryptographyException() : base()
        { }

        public CryptographyException(string message, Exception exception) :
            base(message, exception)
        { }
    }
}
