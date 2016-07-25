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
        public bool IsTokenExpired => !IsTokenValid();

        [JsonExtensionData]
        public Dictionary<string, object> AdditionalTokenParameters { get; } = 
            new Dictionary<string, object>();

        [JsonIgnore]
        public IDictionary<string, string> AdditionalParameters =>
            new ReadOnlyDictionary<string, string>(
                AdditionalTokenParameters.ToDictionary(
                    k => k.Key, 
                    v => v.Value.ToString()));

        private bool IsTokenValid()
        {
            if (string.IsNullOrEmpty(ExpiresIn))
            {
                return true;
            }
            else
            {
                var secondsUntilExpiration = Convert.ToInt32(ExpiresIn);
                if (secondsUntilExpiration == 0)
                {
                    return true;
                }
                else
                {
                    var secondsSinceCreation = (DateTimeOffset.Now - DateCreated).TotalSeconds;
                    return secondsUntilExpiration > secondsSinceCreation;
                }
            }
        }

        public void TimestampToken()
        {
            DateCreated = DateTimeOffset.Now;
        }
    }
}
