using Material.Metadata;
using Material.Infrastructure.ProtectedResources;
using System;
using System.Collections.Generic;
using System.Net;
using Material.Enums;
using Material.Infrastructure;

namespace Material.Infrastructure.Requests
{     
    /// <summary>
    /// Fitness activities appear in a user’s fitness feed on the Runkeeper website
    /// </summary>
    [ServiceType(typeof(Runkeeper))]
	public partial class RunkeeperFitnessActivity : OAuthRequest              
	{
        public override String Host => "https://api.runkeeper.com";
        public override String Path => "/fitnessActivities";
        public override String HttpMethod => "GET";
        public override Dictionary<HttpRequestHeader,String> Headers => new Dictionary<HttpRequestHeader,String> { {HttpRequestHeader.Accept, "application/vnd.com.runkeeper.FitnessActivityFeed+json" } };
        public override List<String> RequiredScopes => new List<String>();
        /// <summary>
        /// Starting ime scope for the request
        /// </summary>
        [Name("noEarlierThan")]
        [ParameterType(RequestParameterTypeEnum.Query)]
        [Format("yyyy-MM-dd")]
        public  Nullable<DateTime> NoEarlierThan { get; set; }
        /// <summary>
        /// Ending time scope for the request
        /// </summary>
        [Name("noLaterThan")]
        [ParameterType(RequestParameterTypeEnum.Query)]
        [Format("yyyy-MM-dd")]
        public  Nullable<DateTime> NoLaterThan { get; set; }
        /// <summary>
        /// The pageSize query parameter controls how many entries are returned per page.
        /// </summary>
        [Name("pageSize")]
        [ParameterType(RequestParameterTypeEnum.Query)]
        public  Nullable<Int32> PageSize { get; set; }
        /// <summary>
        /// The page number to request
        /// </summary>
        [Name("page")]
        [ParameterType(RequestParameterTypeEnum.Query)]
        public  Nullable<Int32> Page { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Name("If-Modified-Since")]
        [ParameterType(RequestParameterTypeEnum.Header)]
        [Format("yyyy-MM-ddTHH:mm:ss")]
        public  Nullable<DateTime> IfModifiedSince { get; set; }
	}
}
