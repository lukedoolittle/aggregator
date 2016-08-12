using System.Runtime.Serialization;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Material.Infrastructure.Responses
{
    [DataContract]
    public class TwitterHashtag
    {
        [DataMember(Name = "text")]
        [JsonProperty(PropertyName = "text")]
        public string Text { get; set; }
        [DataMember(Name = "indices")]
        [JsonProperty(PropertyName = "indices")]
        public IList<int> Indices { get; set; }
    }

    [DataContract]
    public class TwitterUserMention
    {
        [DataMember(Name = "screen_name")]
        [JsonProperty(PropertyName = "screen_name")]
        public string ScreenName { get; set; }
        [DataMember(Name = "name")]
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }
        [DataMember(Name = "id")]
        [JsonProperty(PropertyName = "id")]
        public long Id { get; set; }
        [DataMember(Name = "id_str")]
        [JsonProperty(PropertyName = "id_str")]
        public string IdStr { get; set; }
        [DataMember(Name = "indices")]
        [JsonProperty(PropertyName = "indices")]
        public IList<int> Indices { get; set; }
    }

    [DataContract]
    public class TwitterUrl
    {
        [DataMember(Name = "url")]
        [JsonProperty(PropertyName = "url")]
        public string Url { get; set; }
        [DataMember(Name = "expanded_url")]
        [JsonProperty(PropertyName = "expanded_url")]
        public string ExpandedUrl { get; set; }
        [DataMember(Name = "display_url")]
        [JsonProperty(PropertyName = "display_url")]
        public string DisplayUrl { get; set; }
        [DataMember(Name = "indices")]
        [JsonProperty(PropertyName = "indices")]
        public IList<int> Indices { get; set; }
    }

    [DataContract]
    public class TwitterDescription
    {
        [DataMember(Name = "urls")]
        [JsonProperty(PropertyName = "urls")]
        public IList<TwitterUrl> Urls { get; set; }
    }

    [DataContract]
    public class TwitterEntities
    {
        [DataMember(Name = "hashtags")]
        [JsonProperty(PropertyName = "hashtags")]
        public IList<TwitterHashtag> Hashtags { get; set; }
        [DataMember(Name = "symbols")]
        [JsonProperty(PropertyName = "symbols")]
        public IList<object> Symbols { get; set; }
        [DataMember(Name = "user_mentions")]
        [JsonProperty(PropertyName = "user_mentions")]
        public IList<TwitterUserMention> UserMentions { get; set; }
        [DataMember(Name = "urls")]
        [JsonProperty(PropertyName = "urls")]
        public IList<TwitterUrl> Urls { get; set; }
        [DataMember(Name = "media")]
        [JsonProperty(PropertyName = "media")]
        public IList<TwitterMedium> Media { get; set; }
        [DataMember(Name = "description")]
        [JsonProperty(PropertyName = "description")]
        public TwitterDescription Description { get; set; }
    }

    [DataContract]
    public class TwitterUser
    {
        [DataMember(Name = "id")]
        [JsonProperty(PropertyName = "id")]
        public long Id { get; set; }
        [DataMember(Name = "id_str")]
        [JsonProperty(PropertyName = "id_str")]
        public string IdStr { get; set; }
        [DataMember(Name = "name")]
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }
        [DataMember(Name = "screen_name")]
        [JsonProperty(PropertyName = "screen_name")]
        public string ScreenName { get; set; }
        [DataMember(Name = "location")]
        [JsonProperty(PropertyName = "location")]
        public string Location { get; set; }
        [DataMember(Name = "description")]
        [JsonProperty(PropertyName = "description")]
        public string Description { get; set; }
        [DataMember(Name = "url")]
        [JsonProperty(PropertyName = "url")]
        public string Url { get; set; }
        [DataMember(Name = "entities")]
        [JsonProperty(PropertyName = "entities")]
        public TwitterEntities Entities { get; set; }
        [DataMember(Name = "followers_count")]
        [JsonProperty(PropertyName = "followers_count")]
        public int FollowersCount { get; set; }
        [DataMember(Name = "friends_count")]
        [JsonProperty(PropertyName = "friends_count")]
        public int FriendsCount { get; set; }
        [DataMember(Name = "listed_count")]
        [JsonProperty(PropertyName = "listed_count")]
        public int ListedCount { get; set; }
        [DataMember(Name = "created_at")]
        [JsonProperty(PropertyName = "created_at")]
        public string CreatedAt { get; set; }
        public DateTimeOffset CreatedAtTime => DateTimeOffset.ParseExact(CreatedAt, "ddd MMM dd HH:mm:ss zzz yyyy", null);
        [DataMember(Name = "favourites_count")]
        [JsonProperty(PropertyName = "favourites_count")]
        public int FavouritesCount { get; set; }
        public int? utc_offset { get; set; }
        [DataMember(Name = "time_zone")]
        [JsonProperty(PropertyName = "time_zone")]
        public string TimeZone { get; set; }
        [DataMember(Name = "geo_enabled")]
        [JsonProperty(PropertyName = "geo_enabled")]
        public bool GeoEnabled { get; set; }
        [DataMember(Name = "verified")]
        [JsonProperty(PropertyName = "verified")]
        public bool Verified { get; set; }
        [DataMember(Name = "statuses_count")]
        [JsonProperty(PropertyName = "statuses_count")]
        public int StatusesCount { get; set; }
        [DataMember(Name = "lang")]
        [JsonProperty(PropertyName = "lang")]
        public string Lang { get; set; }
        [DataMember(Name = "contributors_enabled")]
        [JsonProperty(PropertyName = "contributors_enabled")]
        public bool ContributorsEnabled { get; set; }
        [DataMember(Name = "status")]
        [JsonProperty(PropertyName = "status")]
        public TwitterStatus Status { get; set; }
        [DataMember(Name = "is_translator")]
        [JsonProperty(PropertyName = "is_translator")]
        public bool IsTranslator { get; set; }
        [DataMember(Name = "is_translation_enabled")]
        [JsonProperty(PropertyName = "is_translation_enabled")]
        public bool IsTranslationEnabled { get; set; }
        [DataMember(Name = "profile_background_color")]
        [JsonProperty(PropertyName = "profile_background_color")]
        public string ProfileBackgroundColor { get; set; }
        [DataMember(Name = "profile_background_image_url")]
        [JsonProperty(PropertyName = "profile_background_image_url")]
        public string ProfileBackgroundImageUrl { get; set; }
        [DataMember(Name = "profile_background_image_url_https")]
        [JsonProperty(PropertyName = "profile_background_image_url_https")]
        public string ProfileBackgroundImageUrlHttps { get; set; }
        [DataMember(Name = "profile_background_tile")]
        [JsonProperty(PropertyName = "profile_background_tile")]
        public bool ProfileBackgroundTile { get; set; }
        [DataMember(Name = "profile_image_url")]
        [JsonProperty(PropertyName = "profile_image_url")]
        public string ProfileImageUrl { get; set; }
        [DataMember(Name = "profile_image_url_https")]
        [JsonProperty(PropertyName = "profile_image_url_https")]
        public string ProfileImageUrlHttps { get; set; }
        [DataMember(Name = "profile_banner_url")]
        [JsonProperty(PropertyName = "profile_banner_url")]
        public string ProfileBannerUrl { get; set; }
        [DataMember(Name = "profile_link_color")]
        [JsonProperty(PropertyName = "profile_link_color")]
        public string ProfileLinkColor { get; set; }
        [DataMember(Name = "profile_sidebar_border_color")]
        [JsonProperty(PropertyName = "profile_sidebar_border_color")]
        public string ProfileSidebarBorderColor { get; set; }
        [DataMember(Name = "profile_sidebar_fill_color")]
        [JsonProperty(PropertyName = "profile_sidebar_fill_color")]
        public string ProfileSidebarFillColor { get; set; }
        [DataMember(Name = "profile_text_color")]
        [JsonProperty(PropertyName = "profile_text_color")]
        public string ProfileTextColor { get; set; }
        [DataMember(Name = "profile_use_background_image")]
        [JsonProperty(PropertyName = "profile_use_background_image")]
        public bool ProfileUseBackgroundImage { get; set; }
        [DataMember(Name = "has_extended_profile")]
        [JsonProperty(PropertyName = "has_extended_profile")]
        public bool HasExtendedProfile { get; set; }
        [DataMember(Name = "default_profile")]
        [JsonProperty(PropertyName = "default_profile")]
        public bool DefaultProfile { get; set; }
        [DataMember(Name = "default_profile_image")]
        [JsonProperty(PropertyName = "default_profile_image")]
        public bool DefaultProfileImage { get; set; }
        [DataMember(Name = "following")]
        [JsonProperty(PropertyName = "following")]
        public bool Following { get; set; }
        [DataMember(Name = "follow_request_sent")]
        [JsonProperty(PropertyName = "follow_request_sent")]
        public bool FollowRequestSent { get; set; }
        [DataMember(Name = "notifications")]
        [JsonProperty(PropertyName = "notifications")]
        public bool Notifications { get; set; }
        [DataMember(Name = "muting")]
        [JsonProperty(PropertyName = "muting")]
        public bool Muting { get; set; }
        [DataMember(Name = "blocking")]
        [JsonProperty(PropertyName = "blocking")]
        public bool Blocking { get; set; }
        [DataMember(Name = "blocked_by")]
        [JsonProperty(PropertyName = "blocked_by")]
        public bool BlockedBy { get; set; }
    }

    [DataContract]
    public class TwitterStatus
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
        [DataMember(Name = "retweeted_status")]
        [JsonProperty(PropertyName = "retweeted_status")]
        public TwitterRetweetedStatus RetweetedStatus { get; set; }
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
        [DataMember(Name = "lang")]
        [JsonProperty(PropertyName = "lang")]
        public string Lang { get; set; }
    }

    [DataContract]
    public class TwitterBoundingBox
    {
        [DataMember(Name = "type")]
        [JsonProperty(PropertyName = "type")]
        public string Type { get; set; }
        public IList<IList<IList<double>>> coordinates { get; set; }
    }

    [DataContract]
    public class TwitterPlace
    {
        [DataMember(Name = "id")]
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }
        [DataMember(Name = "url")]
        [JsonProperty(PropertyName = "url")]
        public string Url { get; set; }
        [DataMember(Name = "place_type")]
        [JsonProperty(PropertyName = "place_type")]
        public string PlaceType { get; set; }
        [DataMember(Name = "name")]
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }
        [DataMember(Name = "full_name")]
        [JsonProperty(PropertyName = "full_name")]
        public string FullName { get; set; }
        [DataMember(Name = "country_code")]
        [JsonProperty(PropertyName = "country_code")]
        public string CountryCode { get; set; }
        [DataMember(Name = "country")]
        [JsonProperty(PropertyName = "country")]
        public string Country { get; set; }
        [DataMember(Name = "contained_within")]
        [JsonProperty(PropertyName = "contained_within")]
        public IList<object> ContainedWithin { get; set; }
        [DataMember(Name = "bounding_box")]
        [JsonProperty(PropertyName = "bounding_box")]
        public TwitterBoundingBox BoundingBox { get; set; }
        [DataMember(Name = "attributes")]
        [JsonProperty(PropertyName = "attributes")]
        public object Attributes { get; set; }
    }

    [DataContract]
    public class TwitterGeo
    {
        [DataMember(Name = "type")]
        [JsonProperty(PropertyName = "type")]
        public string Type { get; set; }
        [DataMember(Name = "coordinates")]
        [JsonProperty(PropertyName = "coordinates")]
        public IList<double> Coordinates { get; set; }
    }

    [DataContract]
    public class TwitterCoordinates
    {
        [DataMember(Name = "type")]
        [JsonProperty(PropertyName = "type")]
        public string Type { get; set; }
        [DataMember(Name = "coordinates")]
        [JsonProperty(PropertyName = "coordinates")]
        public IList<double> Coordinates { get; set; }
    }

    [DataContract]
    public class TwitterSize
    {
        [DataMember(Name = "w")]
        [JsonProperty(PropertyName = "w")]
        public int W { get; set; }
        [DataMember(Name = "h")]
        [JsonProperty(PropertyName = "h")]
        public int H { get; set; }
        [DataMember(Name = "resize")]
        [JsonProperty(PropertyName = "resize")]
        public string Resize { get; set; }
    }

    [DataContract]
    public class TwitterMedium
    {
        [DataMember(Name = "id")]
        [JsonProperty(PropertyName = "id")]
        public object Id { get; set; }
        [DataMember(Name = "id_str")]
        [JsonProperty(PropertyName = "id_str")]
        public string IdStr { get; set; }
        [DataMember(Name = "indices")]
        [JsonProperty(PropertyName = "indices")]
        public IList<int> Indices { get; set; }
        [DataMember(Name = "media_url")]
        [JsonProperty(PropertyName = "media_url")]
        public string MediaUrl { get; set; }
        [DataMember(Name = "media_url_https")]
        [JsonProperty(PropertyName = "media_url_https")]
        public string MediaUrlHttps { get; set; }
        [DataMember(Name = "url")]
        [JsonProperty(PropertyName = "url")]
        public string Url { get; set; }
        [DataMember(Name = "display_url")]
        [JsonProperty(PropertyName = "display_url")]
        public string DisplayUrl { get; set; }
        [DataMember(Name = "expanded_url")]
        [JsonProperty(PropertyName = "expanded_url")]
        public string ExpandedUrl { get; set; }
        [DataMember(Name = "type")]
        [JsonProperty(PropertyName = "type")]
        public string Type { get; set; }
        public Dictionary<string, TwitterSize> sizes { get; set; }
        [DataMember(Name = "ext_alt_text")]
        [JsonProperty(PropertyName = "ext_alt_text")]
        public object ExtAltText { get; set; }
        public long? source_status_id { get; set; }
        [DataMember(Name = "source_status_id_str")]
        [JsonProperty(PropertyName = "source_status_id_str")]
        public string SourceStatusIdStr { get; set; }
        public long? source_user_id { get; set; }
        [DataMember(Name = "source_user_id_str")]
        [JsonProperty(PropertyName = "source_user_id_str")]
        public string SourceUserIdStr { get; set; }
        [DataMember(Name = "video_info")]
        [JsonProperty(PropertyName = "video_info")]
        public TwitterVideoInfo VideoInfo { get; set; }
    }

    [DataContract]
    public class TwitterVariant
    {
        [DataMember(Name = "bitrate")]
        [JsonProperty(PropertyName = "bitrate")]
        public int Bitrate { get; set; }
        [DataMember(Name = "content_type")]
        [JsonProperty(PropertyName = "content_type")]
        public string ContentType { get; set; }
        [DataMember(Name = "url")]
        [JsonProperty(PropertyName = "url")]
        public string Url { get; set; }
    }

    [DataContract]
    public class TwitterVideoInfo
    {
        [DataMember(Name = "aspect_ratio")]
        [JsonProperty(PropertyName = "aspect_ratio")]
        public IList<int> AspectRatio { get; set; }
        [DataMember(Name = "variants")]
        [JsonProperty(PropertyName = "variants")]
        public IList<TwitterVariant> Variants { get; set; }
    }

    [DataContract]
    public class TwitterExtendedEntities
    {
        [DataMember(Name = "media")]
        [JsonProperty(PropertyName = "media")]
        public IList<TwitterMedium> Media { get; set; }
    }

    [DataContract]
    public class TwitterRetweetedStatus
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
        [DataMember(Name = "source")]
        [JsonProperty(PropertyName = "source")]
        public string Source { get; set; }
        [DataMember(Name = "in_reply_to_status_id")]
        [JsonProperty(PropertyName = "in_reply_to_status_id")]
        public object InReplyToStatusId { get; set; }
        [DataMember(Name = "in_reply_to_status_id_str")]
        [JsonProperty(PropertyName = "in_reply_to_status_id_str")]
        public object InReplyToStatusIdStr { get; set; }
        [DataMember(Name = "in_reply_to_user_id")]
        [JsonProperty(PropertyName = "in_reply_to_user_id")]
        public object InReplyToUserId { get; set; }
        [DataMember(Name = "in_reply_to_user_id_str")]
        [JsonProperty(PropertyName = "in_reply_to_user_id_str")]
        public object InReplyToUserIdStr { get; set; }
        [DataMember(Name = "in_reply_to_screen_name")]
        [JsonProperty(PropertyName = "in_reply_to_screen_name")]
        public object InReplyToScreenName { get; set; }
        [DataMember(Name = "user")]
        [JsonProperty(PropertyName = "user")]
        public TwitterUser User { get; set; }
        [DataMember(Name = "geo")]
        [JsonProperty(PropertyName = "geo")]
        public object Geo { get; set; }
        [DataMember(Name = "coordinates")]
        [JsonProperty(PropertyName = "coordinates")]
        public object Coordinates { get; set; }
        [DataMember(Name = "place")]
        [JsonProperty(PropertyName = "place")]
        public object Place { get; set; }
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
        [DataMember(Name = "lang")]
        [JsonProperty(PropertyName = "lang")]
        public string Lang { get; set; }
        public bool? possibly_sensitive { get; set; }
        public bool? possibly_sensitive_appealable { get; set; }
    }
}
