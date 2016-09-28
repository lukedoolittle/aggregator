using System.Text;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Digests;
using Org.BouncyCastle.Crypto.Macs;
using Org.BouncyCastle.Crypto.Parameters;

namespace Foundations.Cryptography.JsonWebToken
{
    public class DigestSigningAlgorithm : ISigningAlgorithm
    {
        private readonly IDigest _digest;

        public DigestSigningAlgorithm(IDigest digest)
        {
            _digest = digest;
        }

        public byte[] SignText(byte[] text, string privateKey)
        {
            var hmac = new HMac(_digest);
            hmac.Init(new KeyParameter(Encoding.UTF8.GetBytes(privateKey)));
            var result = new byte[hmac.GetMacSize()];

            hmac.BlockUpdate(text, 0, text.Length);
            hmac.DoFinal(result, 0);

            return result;
        }

        public static ISigningAlgorithm Sha1Algorithm()
        {
            return new DigestSigningAlgorithm(new Sha1Digest());
        }
    }
}
