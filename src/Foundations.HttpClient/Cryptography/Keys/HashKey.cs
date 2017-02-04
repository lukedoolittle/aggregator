using Foundations.HttpClient.Cryptography.Enums;

namespace Foundations.HttpClient.Cryptography.Keys
{
    public class HashKey : CryptoKey
    {
        public HashKey(
            string key, 
            StringEncoding encoding) : 
                base(
                    key, 
                    null,
                    encoding)
        { }
    }
}
