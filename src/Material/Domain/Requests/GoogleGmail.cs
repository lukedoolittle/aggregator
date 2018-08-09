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
    /// Gets the specified message
    /// </summary>
    [ServiceType(typeof(Google))]
	[GeneratedCode("T4Toolbox", "14.0")]
	public partial class GoogleGmail : OAuthRequest              
	{
        public override String Host => "https://www.googleapis.com";
        public override String Path => "/gmail/v1/users/me/messages/{messageId}";
        public override String HttpMethod => "GET";
        public override List<MediaType> Produces => new List<MediaType> { MediaType.Json };
        public override List<MediaType> Consumes => new List<MediaType> { MediaType.Json };
        public override List<HttpStatusCode> ExpectedStatusCodes => new List<HttpStatusCode> { HttpStatusCode.OK };
        public override List<String> RequiredScopes => new List<String> { "https://www.googleapis.com/auth/gmail.readonly" };
        /// <summary>
        /// The ID of the message to retrieve
        /// </summary>
        [Name("messageId")]
        [ParameterType(RequestParameterType.Path)]
        [Required()]
        [DefaultFormatter()]
        public  String MessageId { get; set; }
	}
}