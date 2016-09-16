using Material.Metadata;
using Material.Infrastructure.ProtectedResources;
using System;
using System.Collections.Generic;
using Material.Enums;
using Material.Infrastructure;

namespace Material.Infrastructure.Requests
{     
    /// <summary>
    /// The default response returns the ID, link, URL and description of the Pins the authenticated user has liked.
    /// </summary>
    [ServiceType(typeof(Pinterest))]
	public partial class PinterestLikes : OAuthRequest              
	{
        public override String Host => "https://api.pinterest.com";
        public override String Path => "/v1/me/likes";
        public override String HttpMethod => "GET";
        public override List<String> RequiredScopes => new List<String> { "read_public" };
        /// <summary>
        /// The maximum number of objects to return
        /// </summary>
        [Name("limit")]
        [ParameterType(RequestParameterTypeEnum.Query)]
        public  Nullable<Int32> Limit { get; set; } = 25;
        /// <summary>
        /// The starting page of the object to return
        /// </summary>
        [Name("cursor")]
        [ParameterType(RequestParameterTypeEnum.Query)]
        public  String Cursor { get; set; }
	}
}
