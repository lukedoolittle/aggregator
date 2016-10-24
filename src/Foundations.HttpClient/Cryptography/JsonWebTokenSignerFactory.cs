using System;
using Org.BouncyCastle.Crypto.Digests;
using Org.BouncyCastle.Security;

namespace Foundations.HttpClient.Cryptography
{
    public class JsonWebTokenSignerFactory : IJsonWebTokenSigningFactory
    {
        public ISigningAlgorithm GetAlgorithm(JsonWebTokenAlgorithm algorithm)
        {
            switch (algorithm)
            {
                case JsonWebTokenAlgorithm.RS256:
                    return new SigningAlgorithm(SignerUtilities.GetSigner("SHA-256withRSA"));
                case JsonWebTokenAlgorithm.RS384:
                    return new SigningAlgorithm(SignerUtilities.GetSigner("SHA-384withRSA"));
                case JsonWebTokenAlgorithm.RS512:
                    return new SigningAlgorithm(SignerUtilities.GetSigner("SHA-384withRSA"));
                case JsonWebTokenAlgorithm.ES256:
                    return new SigningAlgorithm(SignerUtilities.GetSigner("SHA-256withECDSA"));
                case JsonWebTokenAlgorithm.ES384:
                    return new SigningAlgorithm(SignerUtilities.GetSigner("SHA-384withECDSA"));
                case JsonWebTokenAlgorithm.ES512:
                    return new SigningAlgorithm(SignerUtilities.GetSigner("SHA-512withECDSA"));
                case JsonWebTokenAlgorithm.PS256:
                    return new SigningAlgorithm(SignerUtilities.GetSigner("SHA-256withRSAandMGF1"));
                case JsonWebTokenAlgorithm.PS384:
                    return new SigningAlgorithm(SignerUtilities.GetSigner("SHA-384withRSAandMGF1"));
                case JsonWebTokenAlgorithm.PS512:
                    return new SigningAlgorithm(SignerUtilities.GetSigner("SHA-512withRSAandMGF1"));
                case JsonWebTokenAlgorithm.HS256:
                    return new DigestSigningAlgorithm(new Sha256Digest());
                case JsonWebTokenAlgorithm.HS384:
                    return new DigestSigningAlgorithm(new Sha384Digest());
                case JsonWebTokenAlgorithm.HS512:
                    return new DigestSigningAlgorithm(new Sha512Digest());
                default:
                    throw new NotImplementedException();
            }
        }
    }
}
