using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Foundations.HttpClient.Signing
{
    public class SHA256WithRSASigningAlgorithm : IJWTSigningAlgorithm
    {
        public byte[] SignText(byte[] text, string privateKey)
        {
            return Cryptography.Security.RS256(text, privateKey);
        }
    }
}
