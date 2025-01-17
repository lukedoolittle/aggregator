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
    /// 23andMe API 1
    /// </summary>
    [CredentialType(typeof(OAuth2Credentials))]
	[GeneratedCode("T4Toolbox", "14.0")]
	public partial class TwentyThreeAndMe  : OAuth2ResourceProvider 
    {
        public override List<string> AvailableScopes { get; } = new List<string> { "basic", "genomes", "email" };
        public override List<OAuth2FlowType> AllowedFlows { get; } = new List<OAuth2FlowType> { OAuth2FlowType.AccessCode };
        public override List<GrantType> AllowedGrantTypes { get; } = new List<GrantType> { GrantType.AuthCode, GrantType.RefreshToken };
        public override List<OAuth2ResponseType> AllowedResponseTypes { get; } = new List<OAuth2ResponseType> { OAuth2ResponseType.Code };
        public override string TokenName { get; } = "Bearer";
        public override Uri AuthorizationUrl { get; } = new Uri("https://api.23andme.com/authorize/");
        public override Uri TokenUrl { get; } = new Uri("https://api.23andme.com/token/");
        public override bool SupportsPkce { get; } = false;
        public override bool SupportsCustomUrlScheme { get; } = false;
        public override char ScopeDelimiter { get; } = ' ';
    }
}
