using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Foundations.HttpClient.Cryptography
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
