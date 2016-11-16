using System;

namespace Foundations.HttpClient.Cryptography.Keys
{
    public class HashKey : CryptoKey
    {
        public HashKey(string key) : 
            base(key, true)
        { }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "length")]
        public static HashKey Create(int length)
        {
            throw new NotImplementedException();
        }
    }
}
