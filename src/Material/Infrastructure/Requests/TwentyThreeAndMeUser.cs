using Material.Metadata;
using Material.Infrastructure.ProtectedResources;
using System;
using System.Collections.Generic;
using Material.Enums;
using Material.Infrastructure;

namespace Material.Infrastructure.Requests
{     
    /// <summary>
    /// Gets the user id, and a list of profiles (an account can have multiple genotyped people) with ids
    /// </summary>
    [ServiceType(typeof(TwentyThreeAndMe))]
	public partial class TwentyThreeAndMeUser : OAuthRequest              
	{
        public override String Host => "https://api.23andme.com";
        public override String Path => "/1/user/";
        public override String HttpMethod => "GET";
        public override List<String> RequiredScopes => new List<String> { "basic" };
        /// <summary>
        /// return what services are available to the profiles
        /// </summary>
        [Name("services")]
        [ParameterType(RequestParameterTypeEnum.Path)]
        public  Nullable<Boolean> Services { get; set; }
        /// <summary>
        /// request the email for this account
        /// </summary>
        [Name("email")]
        [ParameterType(RequestParameterTypeEnum.Path)]
        public  Nullable<Boolean> Email { get; set; }
	}
}
