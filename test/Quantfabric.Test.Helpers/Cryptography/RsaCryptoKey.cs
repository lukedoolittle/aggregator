#if __WINDOWS__
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Security;

namespace Quantfabric.Test.Helpers.Cryptography
{
    public class RsaCryptoKey
    {
        private readonly AsymmetricCipherKeyPair _keyPair;

        public AsymmetricKeyParameter Public => _keyPair.Public;
        public AsymmetricKeyParameter Private => _keyPair.Private;

        public string Modulus => ((RsaKeyParameters)Public).Modulus.ToString();
        public string Exponent => ((RsaKeyParameters)Public).Exponent.ToString();

        protected RsaCryptoKey(AsymmetricCipherKeyPair keyPair)
        {
            _keyPair = keyPair;
        }

        public static RsaCryptoKey Create()
        {
            RsaKeyPairGenerator r = new RsaKeyPairGenerator();
            r.Init(new KeyGenerationParameters(new SecureRandom(), 1024));
            return new RsaCryptoKey(r.GenerateKeyPair());
        }
    }
}
#endif