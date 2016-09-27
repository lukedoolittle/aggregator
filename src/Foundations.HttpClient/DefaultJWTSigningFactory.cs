using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Foundations.HttpClient
{
    public class DefaultJWTSigningFactory : IJWTSigningFactory
    {
        public IJWTSigningAlgorithm GetAlgorithm(string jwtType)
        {
            throw new NotImplementedException();
        }
    }
}
