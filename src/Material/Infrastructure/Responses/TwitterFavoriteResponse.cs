using Newtonsoft.Json;
using System.Collections.Generic;

namespace Material.Infrastructure.Responses
{
    public class TwitterFavoriteResponse : List<TwitterFavoriteResponseItem> { }

    public class TwitterFavoriteResponseItem
    {
        [JsonProperty(PropertyName = "created_at")]
        public string CreatedAt { get; set; }
        [JsonProperty(PropertyName = "id")]
        public long Id { get; set; }
        [JsonProperty(PropertyName = "id_str")]
        public string IdStr { get; set; }
        [JsonProperty(PropertyName = "text")]
        public string Text { get; set; }
        [JsonProperty(PropertyName = "truncated")]
        public bool Truncated { get; set; }
        [JsonProperty(PropertyName = "entities")]
        public TwitterEntities Entities { get; set; }
        [JsonProperty(PropertyName = "extended_entities")]
        public TwitterExtendedEntities ExtendedEntities { get; set; }
        [JsonProperty(PropertyName = "source")]
        public string Source { get; set; }
        public long? in_reply_to_status_id { get; set; }
        [JsonProperty(PropertyName = "in_reply_to_status_id_str")]
        public string InReplyToStatusIdStr { get; set; }
        public long? in_reply_to_user_id { get; set; }
        [JsonProperty(PropertyName = "in_reply_to_user_id_str")]
        public string InReplyToUserIdStr { get; set; }
        [JsonProperty(PropertyName = "in_reply_to_screen_name")]
        public string InReplyToScreenName { get; set; }
        [JsonProperty(PropertyName = "user")]
        public TwitterUser User { get; set; }
        [JsonProperty(PropertyName = "geo")]
        public TwitterGeo Geo { get; set; }
        [JsonProperty(PropertyName = "coordinates")]
        public TwitterCoordinates Coordinates { get; set; }
        [JsonProperty(PropertyName = "place")]
        public TwitterPlace Place { get; set; }
        [JsonProperty(PropertyName = "contributors")]
        public object Contributors { get; set; }
        [JsonProperty(PropertyName = "is_quote_status")]
        public bool IsQuoteStatus { get; set; }
        [JsonProperty(PropertyName = "retweet_count")]
        public int RetweetCount { get; set; }
        [JsonProperty(PropertyName = "favorite_count")]
        public int FavoriteCount { get; set; }
        [JsonProperty(PropertyName = "favorited")]
        public bool Favorited { get; set; }
        [JsonProperty(PropertyName = "retweeted")]
        public bool Retweeted { get; set; }
        [JsonProperty(PropertyName = "possibly_sensitive")]
        public bool PossiblySensitive { get; set; }
        [JsonProperty(PropertyName = "lang")]
        public string Lang { get; set; }
    }
}
