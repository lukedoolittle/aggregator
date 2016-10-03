using System;
using System.Security.Cryptography;
using System.Text;
using Foundations.Cryptography.DigitalSignature;

namespace Material.Lite
{
    public class DotNetHmacSha1SigningAlgorithm : ISigningAlgorithm
    {
        public string SignatureMethod => "HMAC-SHA1";

        public byte[] SignText(
            byte[] text, 
            string privateKey)
        {
            var hmac = new HMACSHA1(Encoding.UTF8.GetBytes(privateKey));
            hmac.Initialize();
            return hmac.ComputeHash(text);
        }

        public bool VerifyText(
            string publicKey, 
            byte[] signature, 
            byte[] text)
        {
            throw new NotImplementedException("Cannot verify a hash");
        }
    }
}
