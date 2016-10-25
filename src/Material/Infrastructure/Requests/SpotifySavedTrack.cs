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
using Material.Infrastructure;

namespace Material.Infrastructure.Requests
{     
    /// <summary>
    /// Get a list of the songs saved in the current Spotify user’s Your Music library.
    /// </summary>
    [ServiceType(typeof(Spotify))]
	public partial class SpotifySavedTrack : OAuthRequest              
	{
        public override String Host => "https://api.spotify.com";
        public override String Path => "/v1/me/tracks";
        public override String HttpMethod => "GET";
        public override List<String> RequiredScopes => new List<String> { "user-library-read" };
        /// <summary>
        /// The maximum number of objects to return
        /// </summary>
        [Name("limit")]
        [ParameterType(RequestParameterType.Query)]
        public  Nullable<Int32> Limit { get; set; } = 50;
        /// <summary>
        /// The index of the first object to return
        /// </summary>
        [Name("offset")]
        [ParameterType(RequestParameterType.Query)]
        public  Nullable<Int32> Offset { get; set; }
	}
}
