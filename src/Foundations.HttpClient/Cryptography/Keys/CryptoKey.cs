using System;
using System.Globalization;
using System.IO;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.OpenSsl;

namespace Foundations.HttpClient.Cryptography.Keys
{
    public abstract class CryptoKey
    {
        public string Value { get; }

        protected CryptoKey(
            string key, 
            bool isPrivate)
        {
            if (string.IsNullOrEmpty(key)) throw new ArgumentException("Value cannot be null or empty.", nameof(key));

            if (isPrivate)
            {
                Value = key.Replace(PrivateKeyPrefix, "")
                    .Replace("\n", "")
                    .Replace(PrivateKeySuffix, "");

            }
            else
            {
                Value = key.Replace(PublicKeyPrefix, "")
                    .Replace("\n", "")
                    .Replace(PublicKeySuffix, "");
            }
        }

        protected CryptoKey(AsymmetricKeyParameter key) :
            this(
                key.KeyToString(), 
                key.IsPrivate)
        { }

        public override string ToString()
        {
            return Value;
        }

        protected const string PublicKeyPrefix = "-----BEGIN PUBLIC KEY-----";
        protected const string PublicKeySuffix = "-----END PUBLIC KEY-----";
        protected const string PrivateKeyPrefix = "-----BEGIN PRIVATE KEY-----";
        protected const string PrivateKeySuffix = "-----END PRIVATE KEY-----";
    }

    public static class CryptoKeyExtensions
    {
        public static string KeyToString(
            this AsymmetricKeyParameter key)
        {
            if (key == null) throw new ArgumentNullException(nameof(key));

            object pemObject = key;

            if (key.IsPrivate)
            {
                pemObject = new Pkcs8Generator(key).Generate();
            }

            using (var textWriter = new StringWriter(
                CultureInfo.InvariantCulture))
            {
                var pemWriter = new PemWriter(textWriter);
                pemWriter.WriteObject(pemObject);
                pemWriter.Writer.Flush();

                return textWriter.ToString();
            }
        }
    }
}
