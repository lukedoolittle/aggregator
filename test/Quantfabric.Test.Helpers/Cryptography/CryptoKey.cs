using System.IO;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.OpenSsl;

namespace Quantfabric.Test.Helpers.Cryptography
{
    public static class CryptoKey
    {
        public static string KeyToString(
            this AsymmetricKeyParameter key)
        {
            if (key.IsPrivate)
            {
                var pkcs8Gen = new Pkcs8Generator(key);
                var pemObj = pkcs8Gen.Generate();

                TextWriter textWriter = new StringWriter();
                PemWriter pemWriter = new PemWriter(textWriter);
                pemWriter.WriteObject(pemObj);
                pemWriter.Writer.Flush();

                return textWriter.ToString();
            }
            else
            {
                TextWriter textWriter = new StringWriter();
                PemWriter pemWriter = new PemWriter(textWriter);
                pemWriter.WriteObject(key);
                pemWriter.Writer.Flush();

                return textWriter.ToString();
            }
        }
    }
}
