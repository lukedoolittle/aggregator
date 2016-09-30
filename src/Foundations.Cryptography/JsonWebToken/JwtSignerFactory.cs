using System;
using Foundations.Cryptography.DigitalSignature;
using Org.BouncyCastle.Crypto.Digests;
using Org.BouncyCastle.Security;

namespace Foundations.Cryptography.JsonWebToken
{
    public class JwtSignerFactory : IJwtSigningFactory
    {
        public ISigningAlgorithm GetAlgorithm(JwtAlgorithmEnum algorithm)
        {
            switch (algorithm)
            {
                case JwtAlgorithmEnum.RS256:
                    return new SigningAlgorithm(SignerUtilities.GetSigner("SHA-256withRSA"));
                case JwtAlgorithmEnum.RS384:
                    return new SigningAlgorithm(SignerUtilities.GetSigner("SHA-384withRSA"));
                case JwtAlgorithmEnum.RS512:
                    return new SigningAlgorithm(SignerUtilities.GetSigner("SHA-384withRSA"));
                case JwtAlgorithmEnum.ES256:
                    return new SigningAlgorithm(SignerUtilities.GetSigner("SHA-256withECDSA"));
                case JwtAlgorithmEnum.ES384:
                    return new SigningAlgorithm(SignerUtilities.GetSigner("SHA-384withECDSA"));
                case JwtAlgorithmEnum.ES512:
                    return new SigningAlgorithm(SignerUtilities.GetSigner("SHA-512withECDSA"));
                case JwtAlgorithmEnum.PS256:
                    return new SigningAlgorithm(SignerUtilities.GetSigner("SHA-256withRSAandMGF1"));
                case JwtAlgorithmEnum.PS384:
                    return new SigningAlgorithm(SignerUtilities.GetSigner("SHA-384withRSAandMGF1"));
                case JwtAlgorithmEnum.PS512:
                    return new SigningAlgorithm(SignerUtilities.GetSigner("SHA-512withRSAandMGF1"));
                case JwtAlgorithmEnum.HS256:
                    return new DigestSigningAlgorithm(new Sha256Digest());
                case JwtAlgorithmEnum.HS384:
                    return new DigestSigningAlgorithm(new Sha384Digest());
                case JwtAlgorithmEnum.HS512:
                    return new DigestSigningAlgorithm(new Sha512Digest());
                default:
                    throw new NotImplementedException();
            }
        }
    }
}
