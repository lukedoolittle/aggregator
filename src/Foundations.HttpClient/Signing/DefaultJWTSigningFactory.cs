using System;
using Foundations.HttpClient.Signing;

namespace Foundations.HttpClient
{
    public class DefaultJWTSigningFactory : IJWTSigningFactory
    {
        public IJWTSigningAlgorithm GetAlgorithm(string jwtType)
        {
            if (jwtType == "RS256")
            {
                return new SHA256WithRSASigningAlgorithm();
            }
            else
            {
                throw new NotImplementedException();
            }
        }
    }
}
