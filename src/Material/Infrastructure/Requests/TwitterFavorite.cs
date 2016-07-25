using Material.Metadata;
using Material.Infrastructure.ProtectedResources;
using System;
using Material.Enums;
using Material.Infrastructure;

namespace Material.Infrastructure.Requests
{     
    /// <summary>
    /// Returns the 20 most recent Tweets liked by the authenticating or specified user
    /// </summary>
    [ServiceType(typeof(Twitter))]
	public partial class TwitterFavorite : OAuthRequest              
	{
        public override String Host => "https://api.twitter.com";
        public override String Path => "/1.1/favorites/list.json";
        public override String HttpMethod => "GET";
        /// <summary>
        /// Returns results with an ID greater than (that is, more recent than) the specified ID
        /// </summary>
        [Name("since_id")]
        [ParameterType(RequestParameterTypeEnum.Query)]
        public  String SinceId { get; set; }
        /// <summary>
        /// Returns results with an ID less than (that is, older than) or equal to the specified ID
        /// </summary>
        [Name("max_id")]
        [ParameterType(RequestParameterTypeEnum.Query)]
        public  String MaxId { get; set; }
        /// <summary>
        /// Specifies the number of records to retrieve. Must be less than or equal to 100
        /// </summary>
        [Name("count")]
        [ParameterType(RequestParameterTypeEnum.Query)]
        public  Nullable<Int32> Count { get; set; }
        /// <summary>
        /// The tweet entities node will not be included when set to false
        /// </summary>
        [Name("include_entities")]
        [ParameterType(RequestParameterTypeEnum.Query)]
        public  Nullable<Boolean> IncludeEntities { get; set; }
	}
}
