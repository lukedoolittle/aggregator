using System.CodeDom.Compiler;
using System.Runtime.Serialization;
using System.Collections.Generic;

namespace Material.Infrastructure.Responses
{
    [GeneratedCode("T4Toolbox", "14.0")]
    [DataContract]
    public class TwitterFollowingResponse
    {
        [DataMember(Name = "users")]
        public IList<TwitterUser> Users { get; set; }
        public long? next_cursor { get; set; }
        [DataMember(Name = "next_cursor_str")]
        public string NextCursorStr { get; set; }
        public long? previous_cursor { get; set; }
        [DataMember(Name = "previous_cursor_str")]
        public string PreviousCursorStr { get; set; }
    }
}
