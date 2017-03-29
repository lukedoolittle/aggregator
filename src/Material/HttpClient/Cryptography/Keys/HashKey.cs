using Material.HttpClient.Cryptography.Enums;

namespace Material.HttpClient.Cryptography.Keys
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
