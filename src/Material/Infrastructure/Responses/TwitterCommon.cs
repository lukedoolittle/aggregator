using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Material.Infrastructure.Responses
{
    public class TwitterHashtag
    {
        [JsonProperty(PropertyName = "text")]
        public string Text { get; set; }
        [JsonProperty(PropertyName = "indices")]
        public IList<int> Indices { get; set; }
    }

    public class TwitterUserMention
    {
        [JsonProperty(PropertyName = "screen_name")]
        public string ScreenName { get; set; }
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }
        [JsonProperty(PropertyName = "id")]
        public long Id { get; set; }
        [JsonProperty(PropertyName = "id_str")]
        public string IdStr { get; set; }
        [JsonProperty(PropertyName = "indices")]
        public IList<int> Indices { get; set; }
    }

    public class TwitterUrl
    {
        [JsonProperty(PropertyName = "url")]
        public string Url { get; set; }
        [JsonProperty(PropertyName = "expanded_url")]
        public string ExpandedUrl { get; set; }
        [JsonProperty(PropertyName = "display_url")]
        public string DisplayUrl { get; set; }
        [JsonProperty(PropertyName = "indices")]
        public IList<int> Indices { get; set; }
    }

    public class TwitterDescription
    {
        [JsonProperty(PropertyName = "urls")]
        public IList<TwitterUrl> Urls { get; set; }
    }

    public class TwitterEntities
    {
        [JsonProperty(PropertyName = "hashtags")]
        public IList<TwitterHashtag> Hashtags { get; set; }
        [JsonProperty(PropertyName = "symbols")]
        public IList<object> Symbols { get; set; }
        [JsonProperty(PropertyName = "user_mentions")]
        public IList<TwitterUserMention> UserMentions { get; set; }
        [JsonProperty(PropertyName = "urls")]
        public IList<TwitterUrl> Urls { get; set; }
        [JsonProperty(PropertyName = "media")]
        public IList<TwitterMedium> Media { get; set; }
        [JsonProperty(PropertyName = "description")]
        public TwitterDescription Description { get; set; }
    }

    public class TwitterUser
    {
        [JsonProperty(PropertyName = "id")]
        public long Id { get; set; }
        [JsonProperty(PropertyName = "id_str")]
        public string IdStr { get; set; }
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }
        [JsonProperty(PropertyName = "screen_name")]
        public string ScreenName { get; set; }
        [JsonProperty(PropertyName = "location")]
        public string Location { get; set; }
        [JsonProperty(PropertyName = "description")]
        public string Description { get; set; }
        [JsonProperty(PropertyName = "url")]
        public string Url { get; set; }
        [JsonProperty(PropertyName = "entities")]
        public TwitterEntities Entities { get; set; }
        [JsonProperty(PropertyName = "followers_count")]
        public int FollowersCount { get; set; }
        [JsonProperty(PropertyName = "friends_count")]
        public int FriendsCount { get; set; }
        [JsonProperty(PropertyName = "listed_count")]
        public int ListedCount { get; set; }
        [JsonProperty(PropertyName = "created_at")]
        public string CreatedAt { get; set; }
        public DateTimeOffset CreatedAtTime => DateTimeOffset.ParseExact(CreatedAt, "ddd MMM dd HH:mm:ss zzz yyyy", null);
        [JsonProperty(PropertyName = "favourites_count")]
        public int FavouritesCount { get; set; }
        public int? utc_offset { get; set; }
        [JsonProperty(PropertyName = "time_zone")]
        public string TimeZone { get; set; }
        [JsonProperty(PropertyName = "geo_enabled")]
        public bool GeoEnabled { get; set; }
        [JsonProperty(PropertyName = "verified")]
        public bool Verified { get; set; }
        [JsonProperty(PropertyName = "statuses_count")]
        public int StatusesCount { get; set; }
        [JsonProperty(PropertyName = "lang")]
        public string Lang { get; set; }
        [JsonProperty(PropertyName = "contributors_enabled")]
        public bool ContributorsEnabled { get; set; }
        [JsonProperty(PropertyName = "status")]
        public TwitterStatus Status { get; set; }
        [JsonProperty(PropertyName = "is_translator")]
        public bool IsTranslator { get; set; }
        [JsonProperty(PropertyName = "is_translation_enabled")]
        public bool IsTranslationEnabled { get; set; }
        [JsonProperty(PropertyName = "profile_background_color")]
        public string ProfileBackgroundColor { get; set; }
        [JsonProperty(PropertyName = "profile_background_image_url")]
        public string ProfileBackgroundImageUrl { get; set; }
        [JsonProperty(PropertyName = "profile_background_image_url_https")]
        public string ProfileBackgroundImageUrlHttps { get; set; }
        [JsonProperty(PropertyName = "profile_background_tile")]
        public bool ProfileBackgroundTile { get; set; }
        [JsonProperty(PropertyName = "profile_image_url")]
        public string ProfileImageUrl { get; set; }
        [JsonProperty(PropertyName = "profile_image_url_https")]
        public string ProfileImageUrlHttps { get; set; }
        [JsonProperty(PropertyName = "profile_banner_url")]
        public string ProfileBannerUrl { get; set; }
        [JsonProperty(PropertyName = "profile_link_color")]
        public string ProfileLinkColor { get; set; }
        [JsonProperty(PropertyName = "profile_sidebar_border_color")]
        public string ProfileSidebarBorderColor { get; set; }
        [JsonProperty(PropertyName = "profile_sidebar_fill_color")]
        public string ProfileSidebarFillColor { get; set; }
        [JsonProperty(PropertyName = "profile_text_color")]
        public string ProfileTextColor { get; set; }
        [JsonProperty(PropertyName = "profile_use_background_image")]
        public bool ProfileUseBackgroundImage { get; set; }
        [JsonProperty(PropertyName = "has_extended_profile")]
        public bool HasExtendedProfile { get; set; }
        [JsonProperty(PropertyName = "default_profile")]
        public bool DefaultProfile { get; set; }
        [JsonProperty(PropertyName = "default_profile_image")]
        public bool DefaultProfileImage { get; set; }
        [JsonProperty(PropertyName = "following")]
        public bool Following { get; set; }
        [JsonProperty(PropertyName = "follow_request_sent")]
        public bool FollowRequestSent { get; set; }
        [JsonProperty(PropertyName = "notifications")]
        public bool Notifications { get; set; }
        [JsonProperty(PropertyName = "muting")]
        public bool Muting { get; set; }
        [JsonProperty(PropertyName = "blocking")]
        public bool Blocking { get; set; }
        [JsonProperty(PropertyName = "blocked_by")]
        public bool BlockedBy { get; set; }
    }

    public class TwitterStatus
    {
        [JsonProperty(PropertyName = "created_at")]
        public string CreatedAt { get; set; }
        [JsonProperty(PropertyName = "id")]
        public object Id { get; set; }
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
        [JsonProperty(PropertyName = "geo")]
        public TwitterGeo Geo { get; set; }
        [JsonProperty(PropertyName = "coordinates")]
        public TwitterCoordinates Coordinates { get; set; }
        [JsonProperty(PropertyName = "place")]
        public TwitterPlace Place { get; set; }
        [JsonProperty(PropertyName = "contributors")]
        public object Contributors { get; set; }
        [JsonProperty(PropertyName = "retweeted_status")]
        public TwitterRetweetedStatus RetweetedStatus { get; set; }
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

    public class TwitterBoundingBox
    {
        [JsonProperty(PropertyName = "type")]
        public string Type { get; set; }
        public IList<IList<IList<double>>> coordinates { get; set; }
    }

    public class TwitterPlace
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }
        [JsonProperty(PropertyName = "url")]
        public string Url { get; set; }
        [JsonProperty(PropertyName = "place_type")]
        public string PlaceType { get; set; }
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }
        [JsonProperty(PropertyName = "full_name")]
        public string FullName { get; set; }
        [JsonProperty(PropertyName = "country_code")]
        public string CountryCode { get; set; }
        [JsonProperty(PropertyName = "country")]
        public string Country { get; set; }
        [JsonProperty(PropertyName = "contained_within")]
        public IList<object> ContainedWithin { get; set; }
        [JsonProperty(PropertyName = "bounding_box")]
        public TwitterBoundingBox BoundingBox { get; set; }
        [JsonProperty(PropertyName = "attributes")]
        public object Attributes { get; set; }
    }

    public class TwitterGeo
    {
        [JsonProperty(PropertyName = "type")]
        public string Type { get; set; }
        [JsonProperty(PropertyName = "coordinates")]
        public IList<double> Coordinates { get; set; }
    }

    public class TwitterCoordinates
    {
        [JsonProperty(PropertyName = "type")]
        public string Type { get; set; }
        [JsonProperty(PropertyName = "coordinates")]
        public IList<double> Coordinates { get; set; }
    }

    public class TwitterSize
    {
        [JsonProperty(PropertyName = "w")]
        public int W { get; set; }
        [JsonProperty(PropertyName = "h")]
        public int H { get; set; }
        [JsonProperty(PropertyName = "resize")]
        public string Resize { get; set; }
    }

    public class TwitterMedium
    {
        [JsonProperty(PropertyName = "id")]
        public object Id { get; set; }
        [JsonProperty(PropertyName = "id_str")]
        public string IdStr { get; set; }
        [JsonProperty(PropertyName = "indices")]
        public IList<int> Indices { get; set; }
        [JsonProperty(PropertyName = "media_url")]
        public string MediaUrl { get; set; }
        [JsonProperty(PropertyName = "media_url_https")]
        public string MediaUrlHttps { get; set; }
        [JsonProperty(PropertyName = "url")]
        public string Url { get; set; }
        [JsonProperty(PropertyName = "display_url")]
        public string DisplayUrl { get; set; }
        [JsonProperty(PropertyName = "expanded_url")]
        public string ExpandedUrl { get; set; }
        [JsonProperty(PropertyName = "type")]
        public string Type { get; set; }
        public Dictionary<string, TwitterSize> sizes { get; set; }
        [JsonProperty(PropertyName = "ext_alt_text")]
        public object ExtAltText { get; set; }
        public long? source_status_id { get; set; }
        [JsonProperty(PropertyName = "source_status_id_str")]
        public string SourceStatusIdStr { get; set; }
        public long? source_user_id { get; set; }
        [JsonProperty(PropertyName = "source_user_id_str")]
        public string SourceUserIdStr { get; set; }
        [JsonProperty(PropertyName = "video_info")]
        public TwitterVideoInfo VideoInfo { get; set; }
    }

    public class TwitterVariant
    {
        [JsonProperty(PropertyName = "bitrate")]
        public int Bitrate { get; set; }
        [JsonProperty(PropertyName = "content_type")]
        public string ContentType { get; set; }
        [JsonProperty(PropertyName = "url")]
        public string Url { get; set; }
    }

    public class TwitterVideoInfo
    {
        [JsonProperty(PropertyName = "aspect_ratio")]
        public IList<int> AspectRatio { get; set; }
        [JsonProperty(PropertyName = "variants")]
        public IList<TwitterVariant> Variants { get; set; }
    }

    public class TwitterExtendedEntities
    {
        [JsonProperty(PropertyName = "media")]
        public IList<TwitterMedium> Media { get; set; }
    }

    public class TwitterRetweetedStatus
    {
        [JsonProperty(PropertyName = "created_at")]
        public string CreatedAt { get; set; }
        [JsonProperty(PropertyName = "id")]
        public object Id { get; set; }
        [JsonProperty(PropertyName = "id_str")]
        public string IdStr { get; set; }
        [JsonProperty(PropertyName = "text")]
        public string Text { get; set; }
        [JsonProperty(PropertyName = "truncated")]
        public bool Truncated { get; set; }
        [JsonProperty(PropertyName = "entities")]
        public TwitterEntities Entities { get; set; }
        [JsonProperty(PropertyName = "source")]
        public string Source { get; set; }
        [JsonProperty(PropertyName = "in_reply_to_status_id")]
        public object InReplyToStatusId { get; set; }
        [JsonProperty(PropertyName = "in_reply_to_status_id_str")]
        public object InReplyToStatusIdStr { get; set; }
        [JsonProperty(PropertyName = "in_reply_to_user_id")]
        public object InReplyToUserId { get; set; }
        [JsonProperty(PropertyName = "in_reply_to_user_id_str")]
        public object InReplyToUserIdStr { get; set; }
        [JsonProperty(PropertyName = "in_reply_to_screen_name")]
        public object InReplyToScreenName { get; set; }
        [JsonProperty(PropertyName = "user")]
        public TwitterUser User { get; set; }
        [JsonProperty(PropertyName = "geo")]
        public object Geo { get; set; }
        [JsonProperty(PropertyName = "coordinates")]
        public object Coordinates { get; set; }
        [JsonProperty(PropertyName = "place")]
        public object Place { get; set; }
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
        [JsonProperty(PropertyName = "lang")]
        public string Lang { get; set; }
        public bool? possibly_sensitive { get; set; }
        public bool? possibly_sensitive_appealable { get; set; }
    }
}
