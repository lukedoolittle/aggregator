using System;
using Foundations.HttpClient.Cryptography.Algorithms;
using Foundations.HttpClient.Cryptography.Enums;
using Org.BouncyCastle.Crypto.Digests;
using Org.BouncyCastle.Security;

namespace Foundations.HttpClient.Cryptography
{
    public class JsonWebTokenSignerFactory : IJsonWebTokenSigningFactory
    {
        public ISigningAlgorithm GetSigningAlgorithm(
            JsonWebTokenAlgorithm algorithm)
        {
            return GetAlgorithm<ISigningAlgorithm>(algorithm);
        }

        public IVerificationAlgorithm GetVerificationAlgorithm(
            JsonWebTokenAlgorithm algorithm)
        {
            return GetAlgorithm<IVerificationAlgorithm>(algorithm);
        }

        private static TAlgorithm GetAlgorithm<TAlgorithm>(
            JsonWebTokenAlgorithm algorithm)
            where TAlgorithm : class
        {
            switch (algorithm)
            {
                case JsonWebTokenAlgorithm.RS256:
                    return new SigningAlgorithm(SignerUtilities.GetSigner("SHA-256withRSA")) as TAlgorithm;
                case JsonWebTokenAlgorithm.RS384:
                    return new SigningAlgorithm(SignerUtilities.GetSigner("SHA-384withRSA")) as TAlgorithm;
                case JsonWebTokenAlgorithm.RS512:
                    return new SigningAlgorithm(SignerUtilities.GetSigner("SHA-512withRSA")) as TAlgorithm;
                case JsonWebTokenAlgorithm.ES256:
                    return new EllipticCurveSigningAlgorithm(SignerUtilities.GetSigner("SHA-256withECDSA")) as TAlgorithm;
                case JsonWebTokenAlgorithm.ES384:
                    return new EllipticCurveSigningAlgorithm(SignerUtilities.GetSigner("SHA-384withECDSA")) as TAlgorithm;
                case JsonWebTokenAlgorithm.ES512:
                    return new EllipticCurveSigningAlgorithm(SignerUtilities.GetSigner("SHA-512withECDSA")) as TAlgorithm;
                case JsonWebTokenAlgorithm.HS256:
                    return new DigestSigningAlgorithm(new Sha256Digest()) as TAlgorithm;
                case JsonWebTokenAlgorithm.HS384:
                    return new DigestSigningAlgorithm(new Sha384Digest()) as TAlgorithm;
                case JsonWebTokenAlgorithm.HS512:
                    return new DigestSigningAlgorithm(new Sha512Digest()) as TAlgorithm;
                //These have not been tested and I have no idea if they work
                //case JsonWebTokenAlgorithm.PS256:
                //    return new SigningAlgorithm(SignerUtilities.GetSigner("SHA-256withRSAandMGF1")) as TAlgorithm;
                //case JsonWebTokenAlgorithm.PS384:
                //    return new SigningAlgorithm(SignerUtilities.GetSigner("SHA-384withRSAandMGF1")) as TAlgorithm;
                //case JsonWebTokenAlgorithm.PS512:
                //    return new SigningAlgorithm(SignerUtilities.GetSigner("SHA-512withRSAandMGF1")) as TAlgorithm;
                default:
                    throw new NotSupportedException();
            }
        }
    }
}
