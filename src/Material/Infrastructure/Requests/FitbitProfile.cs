using Material.Metadata;
using Material.Infrastructure.ProtectedResources;
using System;
using System.Collections.Generic;
using Material.Infrastructure;

namespace Material.Infrastructure.Requests
{     
    /// <summary>
    /// The Get Profile endpoint returns a user's profile
    /// </summary>
    [ServiceType(typeof(Fitbit))]
	public partial class FitbitProfile : OAuthRequest              
	{
        public override String Host => "https://api.fitbit.com";
        public override String Path => "/1/user/-/profile.json";
        public override String HttpMethod => "GET";
        public override List<String> RequiredScopes => new List<String> { "profile" };
	}
}
