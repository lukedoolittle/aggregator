using Material.Metadata;
using Material.Infrastructure.ProtectedResources;
using System;
using Material.Enums;
using Material.Infrastructure;

namespace Material.Infrastructure.Requests
{     
    /// <summary>
    /// Returns the 20 most recent mentions (tweets containing a users’s @screen_name) for the authenticating user
    /// </summary>
    [ServiceType(typeof(Twitter))]
	public partial class TwitterMention : OAuthRequest              
	{
        public override String Host => "https://api.twitter.com";
        public override String Path => "/1.1/statuses/mentions_timeline.json";
        public override String HttpMethod => "GET";
        /// <summary>
        /// Returns results with an ID greater than (that is, more recent than) the specified ID
        /// </summary>
        [Name("since_id")]
        [ParameterType(RequestParameterTypeEnum.Query)]
        public  String SinceId { get; set; }
        /// <summary>
        /// Returns results with an ID less than (that is, older than) or equal to the specified ID
        /// </summary>
        [Name("max_id")]
        [ParameterType(RequestParameterTypeEnum.Query)]
        public  String MaxId { get; set; }
        /// <summary>
        /// When true, each tweet returned in a timeline will include a user object including only the status authors numerical ID
        /// </summary>
        [Name("trim_user")]
        [ParameterType(RequestParameterTypeEnum.Query)]
        public  Nullable<Boolean> TrimUser { get; set; }
        /// <summary>
        /// The tweet entities node will not be included when set to false
        /// </summary>
        [Name("include_entities")]
        [ParameterType(RequestParameterTypeEnum.Query)]
        public  Nullable<Boolean> IncludeEntities { get; set; }
	}
}
