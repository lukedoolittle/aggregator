using Org.BouncyCastle.Asn1.Nist;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Crypto.Parameters;
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

        public static EcdsaCryptoKeyPair Create(string name)
        {
            var generator = new ECKeyPairGenerator();
            var keyGenParam = new ECKeyGenerationParameters(
                NistNamedCurves.GetOid(name),
                new SecureRandom());
            generator.Init(keyGenParam);
            var keyPair = generator.GenerateKeyPair();

            return new EcdsaCryptoKeyPair(
                new EcdsaCryptoKey(
                    (ECPublicKeyParameters)keyPair.Public,
                    name),
                new EcdsaCryptoKey(
                    (ECPrivateKeyParameters)keyPair.Private,
                    name));
        }
    }
}
