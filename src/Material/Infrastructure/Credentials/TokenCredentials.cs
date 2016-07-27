using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Newtonsoft.Json;

namespace Material.Infrastructure.Credentials
{
    public abstract class TokenCredentials
    {
        public abstract bool HasValidPublicKey { get; }
        public abstract string ExpiresIn { get; }
        public abstract bool AreValidIntermediateCredentials { get; }

        [JsonProperty("user_id")]
        protected string _userId1;
        [JsonProperty("userid")]
        protected string _userId2;
        [JsonIgnore]
        public string UserId => _userId1 ?? _userId2;

        [JsonProperty("created_at")]
        protected string _dateCreated;

        [JsonProperty("dateCreated")]
        public DateTimeOffset DateCreated { get; protected set; }

        [JsonIgnore]
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
                    var secondsUntilExpiration = Convert.ToInt32(ExpiresIn);
                    var secondsSinceCreation = (DateTimeOffset.Now - DateCreated).TotalSeconds;
                    return secondsUntilExpiration > secondsSinceCreation;
                }
            }
        }

        [JsonExtensionData]
        public Dictionary<string, object> AdditionalTokenParameters { get; } = 
            new Dictionary<string, object>();

        [JsonIgnore]
        public IDictionary<string, string> AdditionalParameters =>
            new ReadOnlyDictionary<string, string>(
                AdditionalTokenParameters.ToDictionary(
                    k => k.Key, 
                    v => v.Value.ToString()));

        public void TimestampToken()
        {
            DateCreated = DateTimeOffset.Now;
        }
    }
}
