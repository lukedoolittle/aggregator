using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Security;

namespace Foundations.HttpClient.Cryptography.Keys
{
    public class EcdsaCryptoKeyPair
    {
        public EcdsaCryptoKey Public { get; }
        public EcdsaCryptoKey Private { get; }

        public EcdsaCryptoKeyPair(EcdsaCryptoKey @public, EcdsaCryptoKey @private)
        {
            Public = @public;
            Private = @private;
        }


        public static EcdsaCryptoKeyPair Create()
        {
            return Create(521);
        }

        public static EcdsaCryptoKeyPair Create(int strength)
        {
            var generator = new ECKeyPairGenerator();
            var keyGenParam = new KeyGenerationParameters(
                new SecureRandom(), 
                strength);
            generator.Init(keyGenParam);
            var keyPair = generator.GenerateKeyPair();

            return new EcdsaCryptoKeyPair(
                new EcdsaCryptoKey(keyPair.Public),
                new EcdsaCryptoKey(keyPair.Private));
        }
    }
}
