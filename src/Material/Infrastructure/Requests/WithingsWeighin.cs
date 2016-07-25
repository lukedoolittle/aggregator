using Material.Metadata;
using Material.Infrastructure.ProtectedResources;
using System;
using Material.Enums;
using Material.Infrastructure;

namespace Material.Infrastructure.Requests
{     
    /// <summary>
    /// Get the body measures for a user
    /// </summary>
    [ServiceType(typeof(Withings))]
	public partial class WithingsWeighin : OAuthRequest              
	{
        public override String Host => "https://wbsapi.withings.net";
        public override String Path => "/measure";
        public override String HttpMethod => "GET";
        /// <summary>
        /// the action to take (must be getmeas for this query)
        /// </summary>
        [Name("action")]
        [ParameterType(RequestParameterTypeEnum.Query)]
        public  String Action { get; set; } = "getmeas";
        /// <summary>
        /// the user id of the requester
        /// </summary>
        [Name("userid")]
        [ParameterType(RequestParameterTypeEnum.Query)]
        public  String Userid { get; set; }
        /// <summary>
        /// Returns measures updated or created after a certain date
        /// </summary>
        [Name("lastupdate")]
        [ParameterType(RequestParameterTypeEnum.Query)]
        [Format("ddd")]
        public  Nullable<DateTime> Lastupdate { get; set; }
        /// <summary>
        /// maximum number of measure groups to return
        /// </summary>
        [Name("limit")]
        [ParameterType(RequestParameterTypeEnum.Query)]
        public  Nullable<Int32> Limit { get; set; }
        /// <summary>
        /// skip the offset most recent measure groups
        /// </summary>
        [Name("offset")]
        [ParameterType(RequestParameterTypeEnum.Query)]
        public  Nullable<Int32> Offset { get; set; }
	}
}
