using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Security;

namespace Material.HttpClient.Cryptography.Keys
{
    public class RsaCryptoKeyPair
    {
        public RsaCryptoKey Public { get; }
        public RsaCryptoKey Private { get; }

        public RsaCryptoKeyPair(
            RsaCryptoKey publicKey, 
            RsaCryptoKey privateKey)
        {
            Public = publicKey;
            Private = privateKey;
        }

        public static RsaCryptoKeyPair Create(int strength)
        {
            var generator = new RsaKeyPairGenerator();
            generator.Init(new KeyGenerationParameters(
                new SecureRandom(), 
                strength));
            var keyPair = generator.GenerateKeyPair();
            return new RsaCryptoKeyPair(
                new RsaCryptoKey((RsaKeyParameters)keyPair.Public),
                new RsaCryptoKey((RsaKeyParameters)keyPair.Private));
        }
    }
}
