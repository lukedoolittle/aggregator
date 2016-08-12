using System.Runtime.Serialization;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Material.Infrastructure.Responses
{
    [DataContract]
    public class TwitterFollowerResponse
    {
        [DataMember(Name = "users")]
        [JsonProperty(PropertyName = "users")]
        public IList<TwitterUser> Users { get; set; }
        public long? next_cursor { get; set; }
        [DataMember(Name = "next_cursor_str")]
        [JsonProperty(PropertyName = "next_cursor_str")]
        public string NextCursorStr { get; set; }
        public long? previous_cursor { get; set; }
        [DataMember(Name = "previous_cursor_str")]
        [JsonProperty(PropertyName = "previous_cursor_str")]
        public string PreviousCursorStr { get; set; }
    }
}

