using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Security;

namespace Quantfabric.Test.Helpers.Cryptography
{
    public class EcdsaCryptoKey
    {
        private readonly AsymmetricCipherKeyPair _keyPair;

        public AsymmetricKeyParameter Public => _keyPair.Public;
        public AsymmetricKeyParameter Private => _keyPair.Private;

        protected EcdsaCryptoKey(AsymmetricCipherKeyPair keyPair)
        {
            _keyPair = keyPair;
        }

        public static EcdsaCryptoKey Create()
        {
            ECKeyPairGenerator gen = new ECKeyPairGenerator();
            SecureRandom secureRandom = new SecureRandom();
            KeyGenerationParameters keyGenParam = new KeyGenerationParameters(secureRandom, 521);
            gen.Init(keyGenParam);
            return new EcdsaCryptoKey(gen.GenerateKeyPair());
        }
    }
}
