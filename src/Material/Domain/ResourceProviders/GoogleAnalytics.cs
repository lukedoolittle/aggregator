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
    /// Google Analytics Reporting API 4
    /// </summary>
    [CredentialType(typeof(OAuth2Credentials))]
	[GeneratedCode("T4Toolbox", "14.0")]
	public partial class GoogleAnalytics  : OAuth2ResourceProvider 
    {
        public override List<string> AvailableScopes { get; } = new List<string> { "https://www.googleapis.com/auth/analytics.readonly" };
        public override List<OAuth2FlowType> AllowedFlows { get; } = new List<OAuth2FlowType> {  };
        public override List<GrantType> AllowedGrantTypes { get; } = new List<GrantType> { GrantType.JsonWebToken };
        public override List<OAuth2ResponseType> AllowedResponseTypes { get; } = new List<OAuth2ResponseType> {  };
        public override string TokenName { get; } = "Bearer";
        public override Uri AuthorizationUrl { get; } = null;
        public override Uri TokenUrl { get; } = new Uri("https://accounts.google.com/o/oauth2/token");
        public override bool SupportsPkce { get; } = false;
        public override bool SupportsCustomUrlScheme { get; } = false;
        public override char ScopeDelimiter { get; } = ' ';
    }
}