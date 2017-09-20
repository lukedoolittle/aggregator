﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
using Material.Framework.Metadata;
using Material.Domain.ResourceProviders;
using System;
using System.Collections.Generic;
using Material.Framework.Enums;
using System.Net;
using Material.Framework.Metadata.Formatters;
using Material.Domain.Core;
using System.CodeDom.Compiler;

namespace Material.Domain.Requests
{     
    /// <summary>
    /// Returns the users that the authenticated user follows. The default response returns the first and last name, ID and URL of the users.
    /// </summary>
    [ServiceType(typeof(Pinterest))]
	[GeneratedCode("T4Toolbox", "14.0")]
	public partial class PinterestFollowing : OAuthRequest              
	{
        public override String Host => "https://api.pinterest.com";
        public override String Path => "/v1/me/following/users";
        public override String HttpMethod => "GET";
        public override List<MediaType> Produces => new List<MediaType> { MediaType.Json };
        public override List<MediaType> Consumes => new List<MediaType> { MediaType.Json };
        public override List<HttpStatusCode> ExpectedStatusCodes => new List<HttpStatusCode> { HttpStatusCode.OK };
        public override List<String> RequiredScopes => new List<String> { "read_relationships" };
        /// <summary>
        /// The maximum number of objects to return
        /// </summary>
        [Name("limit")]
        [ParameterType(RequestParameterType.Query)]
        [DefaultFormatter()]
        public  Nullable<Int32> Limit { get; set; } = 25;
        /// <summary>
        /// The starting page of the object to return
        /// </summary>
        [Name("cursor")]
        [ParameterType(RequestParameterType.Query)]
        [DefaultFormatter()]
        public  String Cursor { get; set; }
	}
}
