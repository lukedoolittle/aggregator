﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
using Material.Metadata;
using Material.Infrastructure.ProtectedResources;
using System;
using Material.Enums;
using Material.Metadata.Formatters;
using Material.Infrastructure;

namespace Material.Infrastructure.Requests
{     
    /// <summary>
    /// Returns a collection of the most recent Tweets posted by the user
    /// </summary>
    [ServiceType(typeof(Twitter))]
	public partial class TwitterTweet : OAuthRequest              
	{
        public override String Host => "https://api.twitter.com";
        public override String Path => "/1.1/statuses/user_timeline.json";
        public override String HttpMethod => "GET";
        /// <summary>
        /// the user id of the requester
        /// </summary>
        [Name("user_id")]
        [ParameterType(RequestParameterType.Query)]
        [DefaultFormatter()]
        public  String UserId { get; set; }
        /// <summary>
        /// Returns results with an ID greater than (that is, more recent than) the specified ID
        /// </summary>
        [Name("since_id")]
        [ParameterType(RequestParameterType.Query)]
        [DefaultFormatter()]
        public  String SinceId { get; set; }
        /// <summary>
        /// Returns results with an ID less than (that is, older than) or equal to the specified ID
        /// </summary>
        [Name("max_id")]
        [ParameterType(RequestParameterType.Query)]
        [DefaultFormatter()]
        public  String MaxId { get; set; }
        /// <summary>
        /// Specifies the number of records to retrieve. Must be less than or equal to 100
        /// </summary>
        [Name("count")]
        [ParameterType(RequestParameterType.Query)]
        [DefaultFormatter()]
        public  Nullable<Int32> Count { get; set; }
        /// <summary>
        /// When true, each tweet returned in a timeline will include a user object including only the status authors numerical ID
        /// </summary>
        [Name("trim_user")]
        [ParameterType(RequestParameterType.Query)]
        [DefaultFormatter()]
        public  Nullable<Boolean> TrimUser { get; set; }
        /// <summary>
        /// The tweet entities node will not be included when set to false
        /// </summary>
        [Name("include_entities")]
        [ParameterType(RequestParameterType.Query)]
        [DefaultFormatter()]
        public  Nullable<Boolean> IncludeEntities { get; set; }
        /// <summary>
        /// The user entities node will not be included when set to false
        /// </summary>
        [Name("include_user_entities")]
        [ParameterType(RequestParameterType.Query)]
        [DefaultFormatter()]
        public  Nullable<Boolean> IncludeUserEntities { get; set; }
        /// <summary>
        /// This parameter will prevent replies from appearing in the returned timeline
        /// </summary>
        [Name("exclude_replies")]
        [ParameterType(RequestParameterType.Query)]
        [DefaultFormatter()]
        public  Nullable<Boolean> ExcludeReplies { get; set; }
        /// <summary>
        /// This parameter enhances the contributors element of the status response to include the screen_name of the contributor
        /// </summary>
        [Name("contributor_details")]
        [ParameterType(RequestParameterType.Query)]
        [DefaultFormatter()]
        public  Nullable<Boolean> ContributorDetails { get; set; }
        /// <summary>
        /// When set to false, the timeline will strip any native retweets
        /// </summary>
        [Name("include_rts")]
        [ParameterType(RequestParameterType.Query)]
        [DefaultFormatter()]
        public  Nullable<Boolean> IncludeRts { get; set; }
	}
}
