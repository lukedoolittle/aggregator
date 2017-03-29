using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Material.Domain.Responses
{
    [GeneratedCode("T4Toolbox", "14.0")]
    public class TwitterTimelineResponse : List<TwitterTimelineItem> { }

    [GeneratedCode("T4Toolbox", "14.0")]
    [DataContract]
    public class TwitterTimelineItem
    {
        [DataMember(Name = "created_at")]
        public string CreatedAt { get; set; }
        [DataMember(Name = "id")]
        public object Id { get; set; }
        [DataMember(Name = "id_str")]
        public string IdStr { get; set; }
        [DataMember(Name = "text")]
        public string Text { get; set; }
        [DataMember(Name = "truncated")]
        public bool Truncated { get; set; }
        [DataMember(Name = "entities")]
        public TwitterEntities Entities { get; set; }
        [DataMember(Name = "extended_entities")]
        public TwitterExtendedEntities ExtendedEntities { get; set; }
        [DataMember(Name = "source")]
        public string Source { get; set; }
        public long? in_reply_to_status_id { get; set; }
        [DataMember(Name = "in_reply_to_status_id_str")]
        public string InReplyToStatusIdStr { get; set; }
        public long? in_reply_to_user_id { get; set; }
        [DataMember(Name = "in_reply_to_user_id_str")]
        public string InReplyToUserIdStr { get; set; }
        [DataMember(Name = "in_reply_to_screen_name")]
        public string InReplyToScreenName { get; set; }
        [DataMember(Name = "user")]
        public TwitterUser User { get; set; }
        [DataMember(Name = "geo")]
        public TwitterGeo Geo { get; set; }
        [DataMember(Name = "coordinates")]
        public TwitterCoordinates Coordinates { get; set; }
        [DataMember(Name = "place")]
        public TwitterPlace Place { get; set; }
        [DataMember(Name = "contributors")]
        public object Contributors { get; set; }
        [DataMember(Name = "is_quote_status")]
        public bool IsQuoteStatus { get; set; }
        [DataMember(Name = "retweet_count")]
        public int RetweetCount { get; set; }
        [DataMember(Name = "favorite_count")]
        public int FavoriteCount { get; set; }
        [DataMember(Name = "favorited")]
        public bool Favorited { get; set; }
        [DataMember(Name = "retweeted")]
        public bool Retweeted { get; set; }
        [DataMember(Name = "possibly_sensitive")]
        public bool PossiblySensitive { get; set; }
        [DataMember(Name = "possibly_sensitive_appealable")]
        public bool PossiblySensitiveAppealable { get; set; }
        [DataMember(Name = "lang")]
        public string Lang { get; set; }
        [DataMember(Name = "retweeted_status")]
        public TwitterRetweetedStatus RetweetedStatus { get; set; }
    }
}
