using System;
using Org.BouncyCastle.Asn1.Nist;
using Org.BouncyCastle.Asn1.Sec;
using Org.BouncyCastle.Asn1.X9;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Math.EC;

namespace Foundations.HttpClient.Cryptography.Keys
{
    public class EcdsaCryptoKey : CryptoKey
    {
        public string AlgorithmName { get; }
        public string CurveName { get; }
        public string XCoordinate { get; }
        public string Coordinate { get; }

        public EcdsaCryptoKey(
            ECPublicKeyParameters key, 
            string curveName) :
                base(key)
        {
            if (key == null) throw new ArgumentNullException(nameof(key));

            AlgorithmName = key.AlgorithmName;
            CurveName = curveName;
            XCoordinate = key.Q.AffineXCoord.ToBigInteger().ToString();
            Coordinate = key.Q.AffineYCoord.ToBigInteger().ToString();
        }

        public EcdsaCryptoKey(
            ECPrivateKeyParameters key,
            string curveName) :
                base(key)
        {
            if (key == null) throw new ArgumentNullException(nameof(key));

            AlgorithmName = key.AlgorithmName;
            CurveName = curveName;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "y")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "x")]
        public EcdsaCryptoKey(
            string algorithmName,
            string curveName, 
            string xCoordinate, 
            string yCoordinate) : 
                base(
                    FromParameters(
                        algorithmName,
                        curveName, 
                        xCoordinate, 
                        yCoordinate))
        { }

        private static AsymmetricKeyParameter FromParameters(
            string algorithmName,
            string curveName, 
            string xCoordinate, 
            string yCoordinate)
        {
            X9ECParameters ecP = NistNamedCurves.GetByName(curveName);
            FpCurve c = (FpCurve)ecP.Curve;
            ECFieldElement x = c.FromBigInteger(new BigInteger(xCoordinate));
            ECFieldElement y = c.FromBigInteger(new BigInteger(yCoordinate));
            ECPoint q = new FpPoint(c, x, y);
            ECPublicKeyParameters publicKeyParameters = 
                new ECPublicKeyParameters(algorithmName, q, NistNamedCurves.GetOid(curveName));
            return publicKeyParameters;
        }
    }
}
