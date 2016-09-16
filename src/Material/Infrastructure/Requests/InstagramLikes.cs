﻿using Material.Metadata;
using Material.Infrastructure.ProtectedResources;
using System;
using System.Collections.Generic;
using Material.Enums;
using Material.Infrastructure;

namespace Material.Infrastructure.Requests
{     
    /// <summary>
    /// Get the list of recent media liked by the owner of the access_token
    /// </summary>
    [ServiceType(typeof(Instagram))]
	public partial class InstagramLikes : OAuthRequest              
	{
        public override String Host => "https://api.instagram.com";
        public override String Path => "/v1/users/self/media/liked";
        public override String HttpMethod => "GET";
        public override List<String> RequiredScopes => new List<String> { "public_content" };
        /// <summary>
        /// Count of media to return
        /// </summary>
        [Name("count")]
        [ParameterType(RequestParameterTypeEnum.Query)]
        public  Nullable<Int32> Count { get; set; }
        /// <summary>
        /// Return media liked before this id
        /// </summary>
        [Name("max_like_id")]
        [ParameterType(RequestParameterTypeEnum.Query)]
        public  String MaxLikeId { get; set; }
	}
}