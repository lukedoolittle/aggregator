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
    /// Returns the 20 most recent direct messages sent to the authenticating user
    /// </summary>
    [ServiceType(typeof(Twitter))]
	public partial class TwitterReceivedDirectMessage : OAuthRequest              
	{
        public override String Host => "https://api.twitter.com";
        public override String Path => "/1.1/direct_messages.json";
        public override String HttpMethod => "GET";
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
        /// The tweet entities node will not be included when set to false
        /// </summary>
        [Name("include_entities")]
        [ParameterType(RequestParameterType.Query)]
        [DefaultFormatter()]
        public  Nullable<Boolean> IncludeEntities { get; set; }
        /// <summary>
        /// When set to either true, t or 1 statuses will not be included in the returned user objects
        /// </summary>
        [Name("skip_status")]
        [ParameterType(RequestParameterType.Query)]
        [DefaultFormatter()]
        public  Nullable<Boolean> SkipStatus { get; set; }
	}
}
