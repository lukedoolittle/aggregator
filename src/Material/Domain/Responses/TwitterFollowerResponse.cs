using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Material.Domain.Responses
{
    [GeneratedCode("T4Toolbox", "14.0")]
    [DataContract]
    public class TwitterFollowerResponse
    {
        [DataMember(Name = "users")]
        public IList<TwitterUserDatum> Users { get; set; }
        public long? next_cursor { get; set; }
        [DataMember(Name = "next_cursor_str")]
        public string NextCursorStr { get; set; }
        public long? previous_cursor { get; set; }
        [DataMember(Name = "previous_cursor_str")]
        public string PreviousCursorStr { get; set; }
    }
}

