using Newtonsoft.Json;
using System.Collections.Generic;

namespace Material.Infrastructure.Responses
{
    public class TwitterFollowerResponse
    {
        [JsonProperty(PropertyName = "users")]
        public IList<TwitterUser> Users { get; set; }
        public long? next_cursor { get; set; }
        [JsonProperty(PropertyName = "next_cursor_str")]
        public string NextCursorStr { get; set; }
        public long? previous_cursor { get; set; }
        [JsonProperty(PropertyName = "previous_cursor_str")]
        public string PreviousCursorStr { get; set; }
    }
}

