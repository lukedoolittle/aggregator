using System;
using System.Globalization;
using Material.Framework.Extensions;
using Material.HttpClient.Cryptography.Algorithms;
using Material.HttpClient.Cryptography.Enums;
using Org.BouncyCastle.Crypto.Digests;
using Org.BouncyCastle.Security;

namespace Material.HttpClient.Cryptography
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
                    return new EllipticCurveSigningAlgorithm(SignerUtilities.GetSigner("SHA-256withECDSA"), "P-256") as TAlgorithm;
                case JsonWebTokenAlgorithm.ES384:
                    return new EllipticCurveSigningAlgorithm(SignerUtilities.GetSigner("SHA-384withECDSA"), "P-384") as TAlgorithm;
                case JsonWebTokenAlgorithm.ES512:
                    return new EllipticCurveSigningAlgorithm(SignerUtilities.GetSigner("SHA-512withECDSA"), "P-521") as TAlgorithm;
                case JsonWebTokenAlgorithm.HS256:
                    return new HmacDigestSigningAlgorithm(new Sha256Digest()) as TAlgorithm;
                case JsonWebTokenAlgorithm.HS384:
                    return new HmacDigestSigningAlgorithm(new Sha384Digest()) as TAlgorithm;
                case JsonWebTokenAlgorithm.HS512:
                    return new HmacDigestSigningAlgorithm(new Sha512Digest()) as TAlgorithm;
                //These have not been tested and I have no idea if they work
                //case JsonWebTokenAlgorithm.PS256:
                //    return new SigningAlgorithm(SignerUtilities.GetSigner("SHA-256withRSAandMGF1")) as TAlgorithm;
                //case JsonWebTokenAlgorithm.PS384:
                //    return new SigningAlgorithm(SignerUtilities.GetSigner("SHA-384withRSAandMGF1")) as TAlgorithm;
                //case JsonWebTokenAlgorithm.PS512:
                //    return new SigningAlgorithm(SignerUtilities.GetSigner("SHA-512withRSAandMGF1")) as TAlgorithm;
                default:
                    throw new NotSupportedException(
                        string.Format(
                            CultureInfo.InvariantCulture, 
                            StringResources.AlgorithmNotSupported, 
                            algorithm.EnumToString()));
            }
        }
    }
}
