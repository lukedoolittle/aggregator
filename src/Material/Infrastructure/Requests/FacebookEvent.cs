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
using System.Collections.Generic;
using Material.Enums;
using Material.Infrastructure.Requests;
using Material.Infrastructure;
using Foundations.Attributes;

namespace Material.Infrastructure.Requests
{     
    /// <summary>
    /// Events for this person. By default this does not include events the person has declined or not replied to.
    /// </summary>
    [ServiceType(typeof(Facebook))]
	public partial class FacebookEvent : OAuthRequest              
	{
        public override String Host => "https://graph.facebook.com";
        public override String Path => "/v2.7/me/events";
        public override String HttpMethod => "GET";
        public override List<String> RequiredScopes => new List<String> { "user_events" };
        /// <summary>
        ///  A Unix timestamp or strtotime data value that points to the start of the range of time-based data
        /// </summary>
        [Name("since")]
        [ParameterType(RequestParameterType.Query)]
        [Format("yyyy-MM-ddTHH:mm:sszzz")]
        public  Nullable<DateTime> Since { get; set; }
        /// <summary>
        ///  A Unix timestamp or strtotime data value that points to the end of the range of time-based data
        /// </summary>
        [Name("until")]
        [ParameterType(RequestParameterType.Query)]
        [Format("yyyy-MM-ddTHH:mm:sszzz")]
        public  Nullable<DateTime> Until { get; set; }
        /// <summary>
        /// This is the number of individual objects that are returned in each page
        /// </summary>
        [Name("limit")]
        [ParameterType(RequestParameterType.Query)]
        public  Nullable<Int32> Limit { get; set; }
        /// <summary>
        /// Query events for which the user has this particular rsvp_status set
        /// </summary>
        [Name("type")]
        [ParameterType(RequestParameterType.Query)]
        public  FacebookEventTypeEnum Type { get; set; }
	}
    public enum FacebookEventTypeEnum
    {
        [Description("attending")] Attending,
        [Description("created")] Created,
        [Description("declined")] Declined,
        [Description("maybe")] Maybe,
        [Description("not_replied")] NotReplied,
    }
}
