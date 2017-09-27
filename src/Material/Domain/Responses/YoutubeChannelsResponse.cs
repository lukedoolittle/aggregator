using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Material.Domain.Responses
{
    [GeneratedCode("T4Toolbox", "14.0")]
    [DataContract]
    public class YoutubePageInfo
    {
        [DataMember(Name = "totalResults")]
        public int TotalResults { get; set; }

        [DataMember(Name = "resultsPerPage")]
        public int ResultsPerPage { get; set; }
    }

    [GeneratedCode("T4Toolbox", "14.0")]
    [DataContract]
    public class YoutubeStatistics
    {
        [DataMember(Name = "viewCount")]
        public string ViewCount { get; set; }

        [DataMember(Name = "commentCount")]
        public string CommentCount { get; set; }

        [DataMember(Name = "subscriberCount")]
        public string SubscriberCount { get; set; }

        [DataMember(Name = "hiddenSubscriberCount")]
        public bool HiddenSubscriberCount { get; set; }

        [DataMember(Name = "videoCount")]
        public string VideoCount { get; set; }
    }

    [GeneratedCode("T4Toolbox", "14.0")]
    [DataContract]
    public class YoutubeChannelsItem
    {
        [DataMember(Name = "kind")]
        public string Kind { get; set; }

        [DataMember(Name = "etag")]
        public string Etag { get; set; }

        [DataMember(Name = "id")]
        public string Id { get; set; }

        [DataMember(Name = "statistics")]
        public YoutubeStatistics Statistics { get; set; }
    }

    [GeneratedCode("T4Toolbox", "14.0")]
    [DataContract]
    public class YoutubeChannelsResponse
    {
        [DataMember(Name = "kind")]
        public string Kind { get; set; }

        [DataMember(Name = "etag")]
        public string Etag { get; set; }

        [DataMember(Name = "pageInfo")]
        public YoutubePageInfo PageInfo { get; set; }

        [DataMember(Name = "items")]
        public IList<YoutubeChannelsItem> Items { get; set; }
    }
}
