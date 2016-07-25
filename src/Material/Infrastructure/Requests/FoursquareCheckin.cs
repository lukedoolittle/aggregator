using Material.Metadata;
using Material.Infrastructure.ProtectedResources;
using System;
using System.Collections.Generic;
using Material.Enums;
using Material.Infrastructure.Requests;
using Material.Infrastructure;
using Foundations.Attributes;

namespace Material.Infrastructure.Requests
{     
    /// <summary>
    /// Returns a history of checkins for the authenticated user
    /// </summary>
    [ServiceType(typeof(Foursquare))]
	public partial class FoursquareCheckin : OAuthRequest              
	{
        public override String Host => "https://api.foursquare.com";
        public override String Path => "/v2/users/self/checkins";
        public override String HttpMethod => "GET";
        public override List<String> RequiredScopes => new List<String>();
        /// <summary>
        /// timestamp of the revision version of the api
        /// </summary>
        [Name("v")]
        [ParameterType(RequestParameterTypeEnum.Query)]
        public  String V { get; set; } = "20140806";
        /// <summary>
        /// platform context for the request
        /// </summary>
        [Name("m")]
        [ParameterType(RequestParameterTypeEnum.Query)]
        public  FoursquareCheckinMEnum M { get; set; } = FoursquareCheckinMEnum.Foursquare;
        /// <summary>
        /// Number of results to return, up to 250
        /// </summary>
        [Name("limit")]
        [ParameterType(RequestParameterTypeEnum.Query)]
        public  Nullable<Int32> Limit { get; set; }
        /// <summary>
        /// The number of results to skip
        /// </summary>
        [Name("offset")]
        [ParameterType(RequestParameterTypeEnum.Query)]
        public  Nullable<Int32> Offset { get; set; }
        /// <summary>
        /// How to sort the returned checkins
        /// </summary>
        [Name("sort")]
        [ParameterType(RequestParameterTypeEnum.Query)]
        public  FoursquareCheckinSortEnum Sort { get; set; } = FoursquareCheckinSortEnum.Newestfirst;
        /// <summary>
        /// Retrieve the first results to follow these seconds since epoch
        /// </summary>
        [Name("afterTimestamp")]
        [ParameterType(RequestParameterTypeEnum.Query)]
        [Format("ddd")]
        public  Nullable<DateTime> AfterTimestamp { get; set; }
        /// <summary>
        /// Retrieve the first results prior to these seconds since epoch
        /// </summary>
        [Name("beforeTimestamp")]
        [ParameterType(RequestParameterTypeEnum.Query)]
        [Format("ddd")]
        public  Nullable<DateTime> BeforeTimestamp { get; set; }
	}
    public enum FoursquareCheckinMEnum
    {
        [Description("foursquare")] Foursquare,
        [Description("swarm")] Swarm,
    }
    public enum FoursquareCheckinSortEnum
    {
        [Description("newestfirst")] Newestfirst,
        [Description("oldestfirst")] Oldestfirst,
    }
}
