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
using Material.Infrastructure;

namespace Material.Infrastructure.Requests
{     
    /// <summary>
    /// Use this method to test if supplied user credentials are valid or to get users email address
    /// </summary>
    [ServiceType(typeof(Twitter))]
	public partial class TwitterVerifyCredentials : OAuthRequest              
	{
        public override String Host => "https://api.twitter.com";
        public override String Path => "/1.1/account/verify_credentials.json";
        public override String HttpMethod => "GET";
        /// <summary>
        /// When set to true statuses will not be included in the returned user object
        /// </summary>
        [Name("skip_status")]
        [ParameterType(RequestParameterTypeEnum.Query)]
        public  Nullable<Boolean> SkipStatus { get; set; }
        /// <summary>
        /// The tweet entities node will not be included when set to false
        /// </summary>
        [Name("include_entities")]
        [ParameterType(RequestParameterTypeEnum.Query)]
        public  Nullable<Boolean> IncludeEntities { get; set; }
        /// <summary>
        /// When set to true email will be returned in the user objects as a string
        /// </summary>
        [Name("include_email")]
        [ParameterType(RequestParameterTypeEnum.Query)]
        public  Nullable<Boolean> IncludeEmail { get; set; } = true;
	}
}