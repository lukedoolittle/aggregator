using Material.Metadata;
using Material.Infrastructure.ProtectedResources;
using System;
using System.Collections.Generic;
using Material.Enums;
using Material.Infrastructure;

namespace Material.Infrastructure.Requests
{     
    /// <summary>
    /// The feed of posts (including status updates) and links published by this person, or by others on this person's profile
    /// </summary>
    [ServiceType(typeof(Facebook))]
	public partial class FacebookFeed : OAuthRequest              
	{
        public override String Host => "https://graph.facebook.com";
        public override String Path => "/v2.7/me/feed";
        public override String HttpMethod => "GET";
        public override List<String> RequiredScopes => new List<String> { "user_posts" };
        /// <summary>
        ///  A Unix timestamp or strtotime data value that points to the start of the range of time-based data
        /// </summary>
        [Name("since")]
        [ParameterType(RequestParameterTypeEnum.Query)]
        [Format("yyyy-MM-ddTHH:mm:sszzz")]
        public  Nullable<DateTime> Since { get; set; }
        /// <summary>
        ///  A Unix timestamp or strtotime data value that points to the end of the range of time-based data
        /// </summary>
        [Name("until")]
        [ParameterType(RequestParameterTypeEnum.Query)]
        [Format("yyyy-MM-ddTHH:mm:sszzz")]
        public  Nullable<DateTime> Until { get; set; }
        /// <summary>
        /// This is the number of individual objects that are returned in each page
        /// </summary>
        [Name("limit")]
        [ParameterType(RequestParameterTypeEnum.Query)]
        public  Nullable<Int32> Limit { get; set; }
	}
}
