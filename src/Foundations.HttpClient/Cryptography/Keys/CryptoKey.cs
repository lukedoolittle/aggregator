using System;
using System.Globalization;
using System.IO;
using Foundations.Extensions;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.OpenSsl;
using Org.BouncyCastle.Security;

namespace Foundations.HttpClient.Cryptography.Keys
{
    public abstract class CryptoKey
    {
        private readonly AsymmetricKeyParameter _parameter;
        private readonly string _value;

        protected CryptoKey(
            string key, 
            bool? isPrivate)
        {
            if (string.IsNullOrEmpty(key)) throw new ArgumentException("Value cannot be null or empty.", nameof(key));

            if (isPrivate.HasValue)
            {
                _value = StripKey(key, isPrivate.Value);
                _parameter = StringToParameters(_value, isPrivate.Value);
            }
            else
            {
                _value = key;
                _parameter = null;
            }
        }

        protected CryptoKey(AsymmetricKeyParameter key)
        {
            if (key == null) throw new ArgumentNullException(nameof(key));

            _parameter = key;
            _value = StripKey(ParametersToString(key), key.IsPrivate);
        }

        public T GetParameter<T>()
            where T : AsymmetricKeyParameter
        {
            if (_parameter != null)
            {
                return (T)_parameter;
            }
            else
            {
                throw new CryptographyException(
                    StringResource.AsymmetricKeyException);
            }
        }

        public override string ToString()
        {
            return _value;
        }

        protected const string PublicKeyPrefix = "-----BEGIN PUBLIC KEY-----";
        protected const string PublicKeySuffix = "-----END PUBLIC KEY-----";
        protected const string PrivateKeyPrefix = "-----BEGIN PRIVATE KEY-----";
        protected const string PrivateKeySuffix = "-----END PRIVATE KEY-----";

        private static AsymmetricKeyParameter StringToParameters(
            string key, 
            bool isPrivate)
        {
            if (isPrivate)
            {
                return PrivateKeyFactory.CreateKey(
                    Convert.FromBase64String(
                        key.ToProperBase64String()));
            }
            else
            {
                return PublicKeyFactory.CreateKey(
                    Convert.FromBase64String(
                        key.ToProperBase64String()));
            }
        }

        private static string ParametersToString(
            AsymmetricKeyParameter key)
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

        private static string StripKey(
            string key, 
            bool isPrivate)
        {
            if (isPrivate)
            {
                return key.Replace(PrivateKeyPrefix, "")
                    .Replace("\n", "")
                    .Replace(PrivateKeySuffix, "");

            }
            else
            {
                return key.Replace(PublicKeyPrefix, "")
                    .Replace("\n", "")
                    .Replace(PublicKeySuffix, "");
            }
        }
    }
}
