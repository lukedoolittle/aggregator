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
using Foundations.HttpClient.Enums;
using Material.Infrastructure;
using System.CodeDom.Compiler;

namespace Material.Infrastructure.ProtectedResources
{     
    /// <summary>
    /// Xamarin Insights Unofficial API 1
    /// </summary>
	[GeneratedCode("T4Toolbox", "14.0")]
	public partial class XamarinInsights : PasswordResourceProvider 
    {
        public override Uri TokenUrl { get; } = new Uri("https://insights.xamarin.com/login");
        public override string UsernameKey { get; } = "username";
        public override string PasswordKey { get; } = "password";

    }
}