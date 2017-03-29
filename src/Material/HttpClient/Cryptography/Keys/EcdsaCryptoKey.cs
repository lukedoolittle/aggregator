using System;
using Material.Framework.Extensions;
using Material.HttpClient.Cryptography.Enums;
using Org.BouncyCastle.Asn1.Nist;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Math.EC;
using Org.BouncyCastle.Utilities.Encoders;

namespace Material.HttpClient.Cryptography.Keys
{
    public class EcdsaCryptoKey : CryptoKey
    {
        public string AlgorithmName { get; }
        public string CurveName { get; }
        public string XCoordinate { get; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1819:PropertiesShouldNotReturnArrays")]
        public byte[] XCoordinateBytes { get; }
        public string YCoordinate { get; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1819:PropertiesShouldNotReturnArrays")]
        public byte[] YCoordinateBytes { get; }

        public EcdsaCryptoKey(
            ECPublicKeyParameters key, 
            string curveName) :
                base(
                    key, 
                    StringEncoding.Base64Url)
        {
            if (key == null) throw new ArgumentNullException(nameof(key));

            AlgorithmName = key.AlgorithmName;
            CurveName = curveName;
            var xCoordinate = key.Q.AffineXCoord.ToBigInteger();
            XCoordinate = xCoordinate.ToString();
            XCoordinateBytes = xCoordinate.ToByteArray();

            var yCoordinate = key.Q.AffineYCoord.ToBigInteger();
            YCoordinate = yCoordinate.ToString();
            YCoordinateBytes = yCoordinate.ToByteArray();
        }

        public EcdsaCryptoKey(
            ECPrivateKeyParameters key,
            string curveName) :
                base(
                    key,
                    StringEncoding.Base64Url)
        {
            if (key == null) throw new ArgumentNullException(nameof(key));

            AlgorithmName = key.AlgorithmName;
            CurveName = curveName;
        }

        public EcdsaCryptoKey(
            string algorithmName,
            string curveName, 
            string xCoordinate, 
            string yCoordinate) : 
                base(
                    FromParameters(
                        algorithmName,
                        curveName,
                        Base64.Decode(xCoordinate.UrlEncodedBase64ToBase64String()),
                        Base64.Decode(yCoordinate.UrlEncodedBase64ToBase64String()))
                    ,StringEncoding.Base64Url)
        { }

        private static AsymmetricKeyParameter FromParameters(
            string algorithmName,
            string curveName,
            byte[] xCoordinate,
            byte[] yCoordinate)
        {
            var curve = NistNamedCurves.GetByName(curveName);

            var c = (FpCurve)curve.Curve;

            var x = c.FromBigInteger(new BigInteger(xCoordinate));
            var y = c.FromBigInteger(new BigInteger(yCoordinate));
            var q = new FpPoint(c, x, y);

            var curveSpec = new ECDomainParameters(
                curve.Curve,
                curve.G,
                curve.N,
                curve.H,
                curve.GetSeed());

            return new ECPublicKeyParameters(
                    algorithmName,
                    q,
                    curveSpec);
        }
    }
}
