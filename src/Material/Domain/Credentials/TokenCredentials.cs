using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Runtime.Serialization;

namespace Material.Domain.Credentials
{
    [DataContract]
    public abstract class TokenCredentials
    {
        public abstract string ExpiresIn { get; }
        public abstract bool AreValidIntermediateCredentials { get; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields")]
        [DataMember(Name = "user_id", EmitDefaultValue = false)]
        protected string _userId1;
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields")]
        [DataMember(Name = "userid", EmitDefaultValue = false)]
        protected string _userId2;

        public string ExternalUserId => _userId1 ?? _userId2;

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields")]
        [DataMember(Name = "created_at", EmitDefaultValue = false)]
        protected string _dateCreated;

        [DataMember(Name = "dateCreated", EmitDefaultValue = false)]
        public DateTimeOffset DateCreated { get; protected set; }

        public bool IsErrorResult { get; set; }

        public bool IsTokenExpired
        {
            get
            {
                if (string.IsNullOrEmpty(ExpiresIn) || ExpiresIn == "0")
                {
                    return false;
                }
                else
                {
                    var secondsUntilExpiration = Convert.ToInt32(
                        ExpiresIn, 
                        CultureInfo.CurrentCulture);
                    var secondsSinceCreation = (DateTimeOffset.Now - DateCreated).TotalSeconds;
                    return secondsSinceCreation > secondsUntilExpiration;
                }
            }
        }

        public Dictionary<string, object> AdditionalTokenParameters { get; } = 
            new Dictionary<string, object>();

        public IDictionary<string, string> AdditionalParameters
        {
            get
            {
                if (AdditionalTokenParameters != null)
                {
                    return new ReadOnlyDictionary<string, string>(
                        AdditionalTokenParameters.ToDictionary(
                            k => k.Key,
                            v => v.Value.ToString()));
                }
                else
                {
                    return new ReadOnlyDictionary<string, string>(
                        new Dictionary<string, string>());
                }
            }
        }
    }
}
