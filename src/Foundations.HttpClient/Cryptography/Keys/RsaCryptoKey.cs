using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Parameters;

namespace Foundations.HttpClient.Cryptography.Keys
{
    public class RsaCryptoKey : CryptoKey
    {
        private readonly AsymmetricKeyParameter _parameter;

        public string Modulus => ((RsaKeyParameters)_parameter).Modulus.ToString();
        public string Exponent => ((RsaKeyParameters)_parameter).Exponent.ToString();

        public RsaCryptoKey(AsymmetricKeyParameter key) :
            base(key)
        {
            _parameter = key;
        }

        public RsaCryptoKey(string key, bool isPrivate) : 
            base(key, isPrivate)
        { }
    }
}
