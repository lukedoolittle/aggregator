using System.CodeDom.Compiler;
using System.Runtime.Serialization;

namespace Material.Domain.Responses
{
    [GeneratedCode("T4Toolbox", "14.0")]
    [DataContract]
    public class TwitterVerifyCredentialsResponse
    {

        [DataMember(Name = "id")]
        public int Id { get; set; }

        [DataMember(Name = "id_str")]
        public string IdStr { get; set; }

        [DataMember(Name = "name")]
        public string Name { get; set; }

        [DataMember(Name = "screen_name")]
        public string ScreenName { get; set; }

        [DataMember(Name = "location")]
        public string Location { get; set; }

        [DataMember(Name = "description")]
        public string Description { get; set; }

        [DataMember(Name = "url")]
        public string Url { get; set; }

        [DataMember(Name = "entities")]
        public TwitterEntities Entities { get; set; }

        [DataMember(Name = "protected")]
        public bool Protected { get; set; }

        [DataMember(Name = "followers_count")]
        public int FollowersCount { get; set; }

        [DataMember(Name = "friends_count")]
        public int FriendsCount { get; set; }

        [DataMember(Name = "listed_count")]
        public int ListedCount { get; set; }

        [DataMember(Name = "created_at")]
        public string CreatedAt { get; set; }

        [DataMember(Name = "favourites_count")]
        public int FavouritesCount { get; set; }

        [DataMember(Name = "utc_offset")]
        public int UtcOffset { get; set; }

        [DataMember(Name = "time_zone")]
        public string TimeZone { get; set; }

        [DataMember(Name = "geo_enabled")]
        public bool GeoEnabled { get; set; }

        [DataMember(Name = "verified")]
        public bool Verified { get; set; }

        [DataMember(Name = "statuses_count")]
        public int StatusesCount { get; set; }

        [DataMember(Name = "lang")]
        public string Lang { get; set; }

        [DataMember(Name = "contributors_enabled")]
        public bool ContributorsEnabled { get; set; }

        [DataMember(Name = "is_translator")]
        public bool IsTranslator { get; set; }

        [DataMember(Name = "is_translation_enabled")]
        public bool IsTranslationEnabled { get; set; }

        [DataMember(Name = "profile_background_color")]
        public string ProfileBackgroundColor { get; set; }

        [DataMember(Name = "profile_background_image_url")]
        public string ProfileBackgroundImageUrl { get; set; }

        [DataMember(Name = "profile_background_image_url_https")]
        public string ProfileBackgroundImageUrlHttps { get; set; }

        [DataMember(Name = "profile_background_tile")]
        public bool ProfileBackgroundTile { get; set; }

        [DataMember(Name = "profile_image_url")]
        public string ProfileImageUrl { get; set; }

        [DataMember(Name = "profile_image_url_https")]
        public string ProfileImageUrlHttps { get; set; }

        [DataMember(Name = "profile_banner_url")]
        public string ProfileBannerUrl { get; set; }

        [DataMember(Name = "profile_link_color")]
        public string ProfileLinkColor { get; set; }

        [DataMember(Name = "profile_sidebar_border_color")]
        public string ProfileSidebarBorderColor { get; set; }

        [DataMember(Name = "profile_sidebar_fill_color")]
        public string ProfileSidebarFillColor { get; set; }

        [DataMember(Name = "profile_text_color")]
        public string ProfileTextColor { get; set; }

        [DataMember(Name = "profile_use_background_image")]
        public bool ProfileUseBackgroundImage { get; set; }

        [DataMember(Name = "has_extended_profile")]
        public bool HasExtendedProfile { get; set; }

        [DataMember(Name = "default_profile")]
        public bool DefaultProfile { get; set; }

        [DataMember(Name = "default_profile_image")]
        public bool DefaultProfileImage { get; set; }

        [DataMember(Name = "following")]
        public bool Following { get; set; }

        [DataMember(Name = "follow_request_sent")]
        public bool FollowRequestSent { get; set; }

        [DataMember(Name = "notifications")]
        public bool Notifications { get; set; }

        [DataMember(Name = "email")]
        public string Email { get; set; }
    }
}
