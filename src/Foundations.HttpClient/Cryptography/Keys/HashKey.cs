using System;

namespace Foundations.HttpClient.Cryptography.Keys
{
    public class HashKey : CryptoKey
    {
        public HashKey(string key) : 
            base(key, true)
        { }

        public static HashKey Create(int length)
        {
            throw new NotImplementedException();
        }
    }
}
