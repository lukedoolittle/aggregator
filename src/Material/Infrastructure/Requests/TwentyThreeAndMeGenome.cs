using Material.Metadata;
using Material.Infrastructure.ProtectedResources;
using System;
using System.Collections.Generic;
using Material.Enums;
using Material.Infrastructure;

namespace Material.Infrastructure.Requests
{     
    /// <summary>
    /// Returns the entire profile's genome as a packed string of base pairs
    /// </summary>
    [ServiceType(typeof(TwentyThreeAndMe))]
	public partial class TwentyThreeAndMeGenome : OAuthRequest              
	{
        public override String Host => "https://api.23andme.com";
        public override String Path => "/1/genomes/{profile_id}/";
        public override String HttpMethod => "GET";
        public override List<String> RequiredScopes => new List<String> { "genomes" };
        /// <summary>
        /// the profile id of the genome holder
        /// </summary>
        [Name("profile_id")]
        [ParameterType(RequestParameterTypeEnum.Path)]
        public  String ProfileId { get; set; }
	}
}
