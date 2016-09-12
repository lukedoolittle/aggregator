using Material.Metadata;
using Material.Infrastructure.ProtectedResources;
using System;
using System.Collections.Generic;
using Material.Infrastructure.Requests;
using Material.Enums;
using Material.Infrastructure;
using Foundations.Attributes;

namespace Material.Infrastructure.Requests
{     
    /// <summary>
    /// https://marketing.adobe.com/developer/documentation/analytics-reporting-1-4/get-started
    /// </summary>
    [ServiceType(typeof(Omniture))]
	public partial class OmnitureReporting : OAuthRequest              
	{
        public override String Host => "https://api2.omniture.com";
        public override String Path => "/admin/1.4/rest";
        public override String HttpMethod => "POST";
        public override List<String> RequiredScopes => new List<String>();
        /// <summary>
        /// The name of the method to call
        /// </summary>
        [Name("method")]
        [ParameterType(RequestParameterTypeEnum.Query)]
        public  OmnitureReportingMethodEnum Method { get; set; }
	}
    public enum OmnitureReportingMethodEnum
    {
        [Description("Cancel")] Cancel,
        [Description("Get")] Get,
        [Description("GetElements")] GetElements,
        [Description("GetMetrics")] GetMetrics,
        [Description("GetQueue")] GetQueue,
        [Description("Run")] Run,
        [Description("Queue")] Queue,
        [Description("Validate")] Validate,
    }
}
