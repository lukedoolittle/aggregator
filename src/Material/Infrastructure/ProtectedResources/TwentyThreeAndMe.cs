﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
using Material.Metadata;
using System;
using Material.Infrastructure.Credentials;
using System.Collections.Generic;
using Foundations.HttpClient.Enums;
using Material.Infrastructure;

namespace Material.Infrastructure.ProtectedResources
{     
    /// <summary>
    /// 23andMe API 1
    /// </summary>
    [CredentialType(typeof(OAuth2Credentials))]
	public partial class TwentyThreeAndMe : OAuth2ResourceProvider              
	{
        public override List<String> AvailableScopes => new List<String> { "basic", "genomes", "email" };
        public override List<OAuth2ResponseType> Flows => new List<OAuth2ResponseType> { OAuth2ResponseType.Code };
        public override List<GrantType> GrantTypes => new List<GrantType> { GrantType.AuthCode, GrantType.RefreshToken };
        public override String TokenName => "Bearer";
        public override Uri AuthorizationUrl => new Uri("https://api.23andme.com/authorize/");
        public override Uri TokenUrl => new Uri("https://api.23andme.com/token/");
	}
}
