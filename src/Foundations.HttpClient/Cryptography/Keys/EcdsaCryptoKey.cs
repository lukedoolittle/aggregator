using Org.BouncyCastle.Crypto;

namespace Foundations.HttpClient.Cryptography.Keys
{
    public class EcdsaCryptoKey : CryptoKey
    {
        public EcdsaCryptoKey(AsymmetricKeyParameter key) : 
            base(key)
        { }
    }
}
