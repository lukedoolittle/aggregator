using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Security;

namespace Foundations.HttpClient.Cryptography.Keys
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

        public static RsaCryptoKeyPair Create()
        {
            return Create(1024);
        }

        public static RsaCryptoKeyPair Create(int strength)
        {
            var generator = new RsaKeyPairGenerator();
            generator.Init(new KeyGenerationParameters(
                new SecureRandom(), 
                strength));
            var keyPair = generator.GenerateKeyPair();
            return new RsaCryptoKeyPair(
                new RsaCryptoKey(keyPair.Public),
                new RsaCryptoKey(keyPair.Private));
        }
    }
}
