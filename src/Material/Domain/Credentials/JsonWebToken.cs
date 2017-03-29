using System;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Security;
using System.Text;
using Material.Framework.Extensions;
using Material.Framework.Serialization;
using Material.HttpClient.Cryptography.Enums;

namespace Material.Domain.Credentials
{
    //The logic within this class clearly violates SOC (hardcoded serializer, etc) however
    //this makes for the simplest implementation
    [DataContract]
    public class JsonWebToken
    {
        public bool IsSigned { get; private set; } = false;

        public JsonWebTokenHeader Header { get; }

        public JsonWebTokenClaims Claims { get; }

        public string EncodedToken => string.Format(
                CultureInfo.InvariantCulture, 
                "{0}.{1}.{2}", 
                Header.Encoded, 
                Claims.Encoded, 
                Signature);

        public string Signature { get; private set; }

        public string SignatureBase
        {
            get
            {
                if (IsSigned)
                {
                    var encodedTokenComponents = EncodedToken.Split('.');
                    return string.Format(
                        CultureInfo.InvariantCulture,
                        "{0}.{1}",
                        encodedTokenComponents[0],
                        encodedTokenComponents[1]);
                }
                else
                {
                    return GetSignatureBase(Header, Claims);
                }
            }
        }

        public JsonWebToken(
            JsonWebTokenHeader header, 
            JsonWebTokenClaims claims, 
            string encodedToken)
        {
            if (header == null) throw new ArgumentNullException(nameof(header));
            if (claims == null) throw new ArgumentNullException(nameof(claims));

            Header = header;
            Claims = claims;
            if (!string.IsNullOrEmpty(encodedToken))
            {
                Sign(encodedToken);
            }
        }

        public JsonWebToken(
            JsonWebTokenHeader header,
            JsonWebTokenClaims claims) : 
                this(header, claims, null)
        { }

        public void Sign(string encodedToken)
        {
            if (string.IsNullOrEmpty(encodedToken))
                throw new ArgumentException("Value cannot be null or empty.", nameof(encodedToken));

            if (IsSigned)
            {
                throw new SecurityException(
                    StringResources.WebTokenAlreadySigned);
            }

            IsSigned = true;
            var components = encodedToken.Split('.');
            Header.Encode(components[0]);
            Claims.Encode(components[1]);
            Signature = components[2];
        }

        public void Sign(string signatureBase, string signature)
        {
            if (string.IsNullOrEmpty(signatureBase))
                throw new ArgumentException("Value cannot be null or empty.", nameof(signatureBase));

            if (IsSigned)
            {
                throw new SecurityException(
                    StringResources.WebTokenAlreadySigned);
            }

            IsSigned = true;
            var components = signatureBase.Split('.');
            Signature = signature;
            Header.Encode(components[0]);
            Claims.Encode(components[1]);
            Signature = signature;
        }

        private static string GetSignatureBase(
            JsonWebTokenHeader header,
            JsonWebTokenClaims claims)
        {
            var serializer = new JsonSerializer();
            var serializedHeader = serializer.Serialize(header);
            var serializedClaims = serializer.Serialize(claims);

            var headerBytes = Encoding.UTF8.GetBytes(serializedHeader);
            var headerEncoded = headerBytes.ToBase64String();

            var claimsBytes = Encoding.UTF8.GetBytes(serializedClaims.Replace("\\", ""));
            var claimsEncoded = claimsBytes.ToBase64String();

            return StringExtensions.Concatenate(
                headerEncoded.TrimEnd('='),
                claimsEncoded.TrimEnd('='),
                ".");
        }
    }

    [DataContract]
    public class JsonWebTokenHeader
    {
        public bool IsReadOnly { get; private set; }
        public string Encoded { get; private set; }

        [DataMember(Name = "typ", EmitDefaultValue = false)]
        private string _mediaType = "JWT";
        //Type of the token provided; the only valid value is JWT
        public string MediaType
        {
            get { return _mediaType; }
            set
            {
                OnPropertyChanged();
                _mediaType = value;
            }
        }

        [DataMember(Name = "alg", EmitDefaultValue = false)]
        private string _algorithm;
        //Algorithm used to sign the token
        public JsonWebTokenAlgorithm Algorithm
        {
            get { return _algorithm.StringToEnum<JsonWebTokenAlgorithm>(); }
            set
            {
                OnPropertyChanged();
                _algorithm = value.EnumToString();
            }
        }

        [DataMember(Name = "kid", EmitDefaultValue = false)]
        private string _signatureKeyId;
        //The identifier of the key used to sign the token
        public string SignatureKeyId
        {
            get { return _signatureKeyId; }
            set
            {
                OnPropertyChanged();
                _signatureKeyId = value;
            }
        }

        public void Encode(string encodedVersion)
        {
            if (IsReadOnly)
            {
                throw new SecurityException(
                    string.Format(
                        CultureInfo.InvariantCulture,
                        StringResources.WebTokenIsReadonly,
                        nameof(Encoded)));
            }

            IsReadOnly = true;
            Encoded = encodedVersion;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1026:DefaultParametersShouldNotBeUsed")]
        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            if (IsReadOnly)
            {
                throw new SecurityException(
                    string.Format(
                        CultureInfo.InvariantCulture, 
                        StringResources.WebTokenIsReadonly, 
                        propertyName));
            }
        }
    }

    [DataContract]
    public class JsonWebTokenClaims
    {
        public bool IsReadOnly { get; private set; }
        public string Encoded { get; private set; }

        [DataMember(Name = "iss", EmitDefaultValue = false)]
        private string _issuer;
        //The Issuer Identifier for the Issuer of the response
        public string Issuer
        {
            get { return _issuer; }
            set
            {
                OnPropertyChanged();
                _issuer = value;
            }
        }

        [DataMember(Name = "iat", EmitDefaultValue = false)]
        private double _issuedAt;
        //The time the ID token was issued, represented in Unix time (integer seconds)
        public DateTime IssuedAt
        {
            get { return _issuedAt.FromUnixTimeSeconds().DateTime; }
            set { _issuedAt = Math.Floor(value.ToUnixTimeSeconds()); }
        }

        [DataMember(Name = "exp", EmitDefaultValue = false)]
        private double _expirationTime;
        //The time the ID token expires, represented in Unix time (integer seconds)
        public DateTime ExpirationTime
        {
            get { return _expirationTime.FromUnixTimeSeconds().DateTime; }
            set { _expirationTime = Math.Floor(value.ToUnixTimeSeconds()); }
        }

        [DataMember(Name = "at_hash", EmitDefaultValue = false)]
        private string _accessTokenHash;
        public string AccessTokenHash
        {
            get { return _accessTokenHash; }
            set
            {
                OnPropertyChanged();
                _accessTokenHash = value;
            }
        }

        [DataMember(Name = "aud", EmitDefaultValue = false)]
        private string _audience;
        //Identifies the audience that this ID token is intended for. Typically one of the client IDs of your application
        public string Audience
        {
            get { return _audience; }
            set
            {
                OnPropertyChanged();
                _audience = value;
            }
        }

        [DataMember(Name = "sub", EmitDefaultValue = false)]
        private string _subject;
        //An identifier for the user, unique among all accounts and never reused
        public string Subject
        {
            get { return _subject; }
            set
            {
                OnPropertyChanged();
                _subject = value;
            }
        }

        [DataMember(Name = "azp", EmitDefaultValue = false)]
        private string _authorizedPresenter;
        //The client_id of the authorized presenter
        public string AuthorizedPresenter
        {
            get { return _authorizedPresenter; }
            set
            {
                OnPropertyChanged();
                _authorizedPresenter = value;
            }
        }

        private string _scope;
        [DataMember(Name = "scope", EmitDefaultValue = false)]
        public string Scope
        {
            get { return _scope; }
            set
            {
                OnPropertyChanged();
                _scope = value;
            }
        }

        private string _nonce;
        [DataMember(Name = "nonce", EmitDefaultValue = false)]
        public string Nonce
        {
            get { return _nonce; }
            set
            {
                OnPropertyChanged();
                _nonce = value;
            }
        }

        private string _clientId;
        [DataMember(Name = "clientId", EmitDefaultValue = false)]
        public string ClientId
        {
            get { return _clientId; }
            set
            {
                OnPropertyChanged();
                _clientId = value;
            }
        }

        private string _bundleId;
        [DataMember(Name = "bundleId", EmitDefaultValue = false)]
        public string BundleId
        {
            get { return _bundleId; }
            set
            {
                OnPropertyChanged();
                _bundleId = value;
            }
        }
        //[DataMember(Name = "nbf", EmitDefaultValue = false)]
        //public string NotBefore { get; set; }

        //[DataMember(Name = "jti", EmitDefaultValue = false)]
        //public string Id { get; set; }

        public void Encode(string encodedVersion)
        {
            if (IsReadOnly)
            {
                throw new SecurityException(
                    string.Format(
                        CultureInfo.InvariantCulture,
                        StringResources.WebTokenIsReadonly,
                        nameof(Encoded)));
            }

            IsReadOnly = true;
            Encoded = encodedVersion;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1026:DefaultParametersShouldNotBeUsed")]
        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            if (IsReadOnly)
            {
                throw new SecurityException(
                    string.Format(
                        CultureInfo.InvariantCulture,
                        StringResources.WebTokenIsReadonly,
                        propertyName));
            }
        }
    }
}
