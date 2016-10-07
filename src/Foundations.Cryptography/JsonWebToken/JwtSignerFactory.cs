using System;
using Foundations.Cryptography.DigitalSignature;
using Org.BouncyCastle.Crypto.Digests;
using Org.BouncyCastle.Security;

namespace Foundations.Cryptography.JsonWebToken
{
    public class JwtSignerFactory : IJwtSigningFactory
    {
        public ISigningAlgorithm GetAlgorithm(JwtAlgorithm algorithm)
        {
            switch (algorithm)
            {
                case JwtAlgorithm.RS256:
                    return new SigningAlgorithm(SignerUtilities.GetSigner("SHA-256withRSA"));
                case JwtAlgorithm.RS384:
                    return new SigningAlgorithm(SignerUtilities.GetSigner("SHA-384withRSA"));
                case JwtAlgorithm.RS512:
                    return new SigningAlgorithm(SignerUtilities.GetSigner("SHA-384withRSA"));
                case JwtAlgorithm.ES256:
                    return new SigningAlgorithm(SignerUtilities.GetSigner("SHA-256withECDSA"));
                case JwtAlgorithm.ES384:
                    return new SigningAlgorithm(SignerUtilities.GetSigner("SHA-384withECDSA"));
                case JwtAlgorithm.ES512:
                    return new SigningAlgorithm(SignerUtilities.GetSigner("SHA-512withECDSA"));
                case JwtAlgorithm.PS256:
                    return new SigningAlgorithm(SignerUtilities.GetSigner("SHA-256withRSAandMGF1"));
                case JwtAlgorithm.PS384:
                    return new SigningAlgorithm(SignerUtilities.GetSigner("SHA-384withRSAandMGF1"));
                case JwtAlgorithm.PS512:
                    return new SigningAlgorithm(SignerUtilities.GetSigner("SHA-512withRSAandMGF1"));
                case JwtAlgorithm.HS256:
                    return new DigestSigningAlgorithm(new Sha256Digest());
                case JwtAlgorithm.HS384:
                    return new DigestSigningAlgorithm(new Sha384Digest());
                case JwtAlgorithm.HS512:
                    return new DigestSigningAlgorithm(new Sha512Digest());
                default:
                    throw new NotImplementedException();
            }
        }
    }
}
