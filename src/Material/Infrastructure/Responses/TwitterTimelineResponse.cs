using System.Runtime.Serialization;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Material.Infrastructure.Responses
{
    public class TwitterTimelineResponse : List<TwitterTimelineItem> { }

    [DataContract]
    public class TwitterTimelineItem
    {
        [DataMember(Name = "created_at")]
        [JsonProperty(PropertyName = "created_at")]
        public string CreatedAt { get; set; }
        [DataMember(Name = "id")]
        [JsonProperty(PropertyName = "id")]
        public object Id { get; set; }
        [DataMember(Name = "id_str")]
        [JsonProperty(PropertyName = "id_str")]
        public string IdStr { get; set; }
        [DataMember(Name = "text")]
        [JsonProperty(PropertyName = "text")]
        public string Text { get; set; }
        [DataMember(Name = "truncated")]
        [JsonProperty(PropertyName = "truncated")]
        public bool Truncated { get; set; }
        [DataMember(Name = "entities")]
        [JsonProperty(PropertyName = "entities")]
        public TwitterEntities Entities { get; set; }
        [DataMember(Name = "extended_entities")]
        [JsonProperty(PropertyName = "extended_entities")]
        public TwitterExtendedEntities ExtendedEntities { get; set; }
        [DataMember(Name = "source")]
        [JsonProperty(PropertyName = "source")]
        public string Source { get; set; }
        public long? in_reply_to_status_id { get; set; }
        [DataMember(Name = "in_reply_to_status_id_str")]
        [JsonProperty(PropertyName = "in_reply_to_status_id_str")]
        public string InReplyToStatusIdStr { get; set; }
        public long? in_reply_to_user_id { get; set; }
        [DataMember(Name = "in_reply_to_user_id_str")]
        [JsonProperty(PropertyName = "in_reply_to_user_id_str")]
        public string InReplyToUserIdStr { get; set; }
        [DataMember(Name = "in_reply_to_screen_name")]
        [JsonProperty(PropertyName = "in_reply_to_screen_name")]
        public string InReplyToScreenName { get; set; }
        [DataMember(Name = "user")]
        [JsonProperty(PropertyName = "user")]
        public TwitterUser User { get; set; }
        [DataMember(Name = "geo")]
        [JsonProperty(PropertyName = "geo")]
        public TwitterGeo Geo { get; set; }
        [DataMember(Name = "coordinates")]
        [JsonProperty(PropertyName = "coordinates")]
        public TwitterCoordinates Coordinates { get; set; }
        [DataMember(Name = "place")]
        [JsonProperty(PropertyName = "place")]
        public TwitterPlace Place { get; set; }
        [DataMember(Name = "contributors")]
        [JsonProperty(PropertyName = "contributors")]
        public object Contributors { get; set; }
        [DataMember(Name = "is_quote_status")]
        [JsonProperty(PropertyName = "is_quote_status")]
        public bool IsQuoteStatus { get; set; }
        [DataMember(Name = "retweet_count")]
        [JsonProperty(PropertyName = "retweet_count")]
        public int RetweetCount { get; set; }
        [DataMember(Name = "favorite_count")]
        [JsonProperty(PropertyName = "favorite_count")]
        public int FavoriteCount { get; set; }
        [DataMember(Name = "favorited")]
        [JsonProperty(PropertyName = "favorited")]
        public bool Favorited { get; set; }
        [DataMember(Name = "retweeted")]
        [JsonProperty(PropertyName = "retweeted")]
        public bool Retweeted { get; set; }
        [DataMember(Name = "possibly_sensitive")]
        [JsonProperty(PropertyName = "possibly_sensitive")]
        public bool PossiblySensitive { get; set; }
        [DataMember(Name = "possibly_sensitive_appealable")]
        [JsonProperty(PropertyName = "possibly_sensitive_appealable")]
        public bool PossiblySensitiveAppealable { get; set; }
        [DataMember(Name = "lang")]
        [JsonProperty(PropertyName = "lang")]
        public string Lang { get; set; }
        [DataMember(Name = "retweeted_status")]
        [JsonProperty(PropertyName = "retweeted_status")]
        public TwitterRetweetedStatus RetweetedStatus { get; set; }
    }
}
