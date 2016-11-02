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
    /// Use this method to retrieve the liked posts that match the OAuth credentials submitted with the request.
    /// </summary>
    [ServiceType(typeof(Tumblr))]
	public partial class TumblrLikes : OAuthRequest              
	{
        public override String Host => "https://api.tumblr.com";
        public override String Path => "/v2/user/likes";
        public override String HttpMethod => "GET";
        /// <summary>
        /// The number of results to return: 1–20, inclusive
        /// </summary>
        [Name("limit")]
        [ParameterType(RequestParameterType.Query)]
        [DefaultFormatter()]
        public  Nullable<Int32> Limit { get; set; } = 20;
        /// <summary>
        /// Liked post number to start at
        /// </summary>
        [Name("offset")]
        [ParameterType(RequestParameterType.Query)]
        [DefaultFormatter()]
        public  String Offset { get; set; }
        /// <summary>
        /// Retrieve posts liked before the specified timestamp
        /// </summary>
        [Name("before")]
        [ParameterType(RequestParameterType.Query)]
        [UnixTimeSecondsDateTimeFormatter()]
        public  Nullable<DateTime> Before { get; set; }
        /// <summary>
        /// Retrieve posts liked after the specified timestamp
        /// </summary>
        [Name("after")]
        [ParameterType(RequestParameterType.Query)]
        [UnixTimeSecondsDateTimeFormatter()]
        public  Nullable<DateTime> After { get; set; }
	}
}
