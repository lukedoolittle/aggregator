using System;
using System.Globalization;
using System.IO;
using System.Text;
using Material.Framework.Extensions;
using Material.HttpClient.Cryptography.Enums;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.OpenSsl;
using Org.BouncyCastle.Security;

namespace Material.HttpClient.Cryptography.Keys
{
    public class CryptoKey
    {
        private readonly AsymmetricKeyParameter _parameter;
        private readonly string _value;
        private readonly StringEncoding _encoding;
        private readonly bool? _isPrivateKey;

        public CryptoKey(
            string key, 
            bool? isPrivate,
            StringEncoding encoding)
        {
            if (string.IsNullOrEmpty(key)) throw new ArgumentException("Value cannot be null or empty.", nameof(key));

            _isPrivateKey = isPrivate;
            _value = key;
            _encoding = encoding;

            if (isPrivate.HasValue)
            {
                _parameter = StringToParameters(_value, isPrivate.Value);
            }
        }

        public CryptoKey(
            AsymmetricKeyParameter key, 
            StringEncoding encoding)
        {
            if (key == null) throw new ArgumentNullException(nameof(key));

            _parameter = key;
            _isPrivateKey = key.IsPrivate;
            _value = ParametersToString(key);
            _encoding = encoding;
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
                    StringResources.AsymmetricKeyException);
            }
        }

        public byte[] GetBytes()
        {
            switch (_encoding)
            {
                case StringEncoding.Utf8:
                    return Encoding.UTF8.GetBytes(ToString());
                case StringEncoding.Base64:
                    return ToString().FromBase64String();
                case StringEncoding.Base64Url:
                    return ToString()
                        .UrlEncodedBase64ToBase64String()
                        .FromBase64String();
                case StringEncoding.Unicode:
                    return Encoding.Unicode.GetBytes(ToString());
                default:
                    throw new NotSupportedException();
            }
        }

        public override string ToString()
        {
            return !_isPrivateKey.HasValue ? 
                _value : 
                StripKey(_value, _isPrivateKey.Value);
        }

        private static AsymmetricKeyParameter StringToParameters(
            string key, 
            bool isPrivate)
        {
            if (isPrivate)
            {
                var value = StripKey(key, true);
                return PrivateKeyFactory.CreateKey(
                    value
                        .UrlEncodedBase64ToBase64String()
                        .FromBase64String());
            }
            else
            {
                var value = StripKey(key, false);
                return PublicKeyFactory.CreateKey(
                    value
                        .UrlEncodedBase64ToBase64String()
                        .FromBase64String());
            }
        }

        private static string ParametersToString(AsymmetricKeyParameter key)
        {
            if (key == null) throw new ArgumentNullException(nameof(key));

            object pemObject = key;

            if (key.IsPrivate)
            {
                pemObject = new Pkcs8Generator(key).Generate();
            }

            using (var textWriter = new StringWriter(CultureInfo.InvariantCulture))
            {
                var pemWriter = new PemWriter(textWriter);
                pemWriter.WriteObject(pemObject);
                pemWriter.Writer.Flush();

                return textWriter.ToString();
            }
        }

        private const string PublicKeyPrefix = "-----BEGIN PUBLIC KEY-----";
        private const string PublicKeySuffix = "-----END PUBLIC KEY-----";
        private const string PrivateKeyPrefix = "-----BEGIN PRIVATE KEY-----";
        private const string PrivateKeySuffix = "-----END PRIVATE KEY-----";

        private static string StripKey(string key, bool isPrivate)
        {
            if (isPrivate)
            {
                return key
                    .Replace(PrivateKeyPrefix, "")
                    .Replace("\n", "")
                    .Replace(PrivateKeySuffix, "");
            }
            else
            {
                return key
                    .Replace(PublicKeyPrefix, "")
                    .Replace("\n", "")
                    .Replace(PublicKeySuffix, "");
            }
        }
    }
}
