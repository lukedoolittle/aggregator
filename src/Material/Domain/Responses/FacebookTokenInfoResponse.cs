using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Material.Domain.Responses
{
    [GeneratedCode("T4Toolbox", "14.0")]
    [DataContract]
    public class FacebookDebugData
    {
        [DataMember(Name = "app_id")]
        public string AppId { get; set; }

        [DataMember(Name = "application")]
        public string Application { get; set; }

        [DataMember(Name = "expires_at")]
        public int ExpiresAt { get; set; }

        [DataMember(Name = "is_valid")]
        public bool IsValid { get; set; }

        [DataMember(Name = "issued_at")]
        public int IssuedAt { get; set; }

        [DataMember(Name = "scopes")]
        public IList<string> Scopes { get; set; }

        [DataMember(Name = "user_id")]
        public string UserId { get; set; }
    }

    [GeneratedCode("T4Toolbox", "14.0")]
    [DataContract]
    public class FacebookTokenInfoResponse
    {
        [DataMember(Name = "data")]
        public FacebookDebugData Data { get; set; }
    }
}
