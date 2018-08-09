﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
using System;
using Material.Domain.Credentials;
using System.Collections.Generic;
using System.CodeDom.Compiler;
using Material.Framework.Enums;
using Material.Framework.Metadata;
using Material.Domain.Core;

namespace Material.Domain.ResourceProviders
{     
    /// <summary>
    /// AppFigures API v2.0
    /// </summary>
    [CredentialType(typeof(OAuth1Credentials))]
	[GeneratedCode("T4Toolbox", "14.0")]
	public partial class AppFigures  : OAuth1ResourceProvider 
    {
        public override Uri RequestUrl { get; } = new Uri("https://api.appfigures.com/v2/oauth/request_token");
        public override Uri AuthorizationUrl { get; } = new Uri("https://api.appfigures.com/v2/oauth/authorize");
        public override Uri TokenUrl { get; } = new Uri("https://api.appfigures.com/v2/oauth/access_token");
        public override HttpParameterType ParameterType { get; } = HttpParameterType.Body;
        public override bool SupportsCustomUrlScheme { get; } = false;
    }
}